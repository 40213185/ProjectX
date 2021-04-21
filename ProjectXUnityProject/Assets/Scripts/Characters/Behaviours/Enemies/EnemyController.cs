using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //stats
    public Stats stats { get; private set; }
    //highlighting cells
    private HighlightCells highlight;

    public enum State {
    Idle,
    Wait,
    Move,
    Action,
    TurnEnd,
    Dead
    }

    public State actionState;
    private bool myTurn; //used to update the controller. Only updates when true

    public EnemyLibrary.EnemyType enemyType;
    public float movementSpeed;
    private Vector3 nextPoint;
    public float moveWaitTime;
    public float attackWaitTime;
    private float waitTimer;
    private bool wait;

    private Vector2Int[] movePoints;
    private int pointPick;
    private int weaponPick;
    private bool moved;
    private bool usingAoe ;
    private Vector2Int aoePoint;
    private List<int> aoePointsWeapon;
    private List<Vector2Int> aoePoints;

    //equipment
    private Weapon[] weapons;
    private List<int> weaponChoice;
    private bool withinAttackRange;
    private bool actionComplete;

    //player reference
    private GameObject player;

    //animation
    public GameObject animationObject;
    private EnemyAnimationController animationController;

    //particles
    public GameObject deathParticles;
    private bool usedDeathParticles;

    public void SetStats() 
    {
        stats = EnemyLibrary.GetEnemyStats(enemyType);
    }

    // Start is called before the first frame update
    void Start()
    {
        //actionState = State.Idle;
        SetStats();

        player = GameObject.FindGameObjectWithTag("Player");

        waitTimer = Time.time + moveWaitTime;
        wait = false;

        //weapons
        weapons = EnemyLibrary.GetEnemyWeaponry(enemyType);
        weaponChoice = new List<int>();
        withinAttackRange = false;
        aoePoints = new List<Vector2Int>();
        aoePointsWeapon = new List<int>();

        //highlighting
        highlight = HighlightCells.instance;

        //animation
        animationController = animationObject.GetComponent<EnemyAnimationController>();

        usedDeathParticles = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stats.GetCurrentHealth() <= 0) actionState = State.Dead;
        if (myTurn)
        {
            switch (actionState)
            {
                case State.Wait:
                    {
                        //check end turn conditions
                        if ((moved && !canUseWeapon()) || actionComplete)
                        {
                            actionState = State.TurnEnd;
                            break;
                        }
                        //setup for weapon decision
                        weaponChoice.Clear();
                        aoePoints.Clear();
                        aoePointsWeapon.Clear();

                        Vector2Int playerPos = new Vector2Int(Mathf.FloorToInt(player.transform.position.x),
                            Mathf.FloorToInt(player.transform.position.z));
                        bool moveDecision = false;
                        bool inrange = false;
                        withinAttackRange = false;
                        usingAoe = false;
                        for (int i = 0; i < weapons.Length; i++)
                        {
                            aoePoint = new Vector2Int();
                            //check if within weapon range + aoe
                            foreach (Vector2Int v in weapons[i].GetRangeTiles(GetMatrixPos()))
                            {
                                if (!MapHandler.isSightBlocked(
                                    transform.position, player.transform.position))
                                {
                                    //check area of effect
                                    foreach (Vector2Int t in weapons[i].GetAoeTiles(v, GetMatrixPos()))
                                    {
                                        if (playerPos == t)
                                        {
                                            inrange = true;
                                            usingAoe = true;
                                            aoePoint = v;
                                            break;
                                        }
                                    }
                                    if (playerPos == v)
                                    {
                                        inrange = true;
                                        break;
                                    }
                                }
                            }
                            if (inrange)
                            {
                                //add weapon possibility to list
                                if (usingAoe)
                                {
                                    aoePoints.Add(aoePoint);
                                    aoePointsWeapon.Add(i);
                                }
                                else {
                                    weaponChoice.Add(i);
                                }
                                //set to attack
                                withinAttackRange = true;
                            }
                        }
                        //if above fails check with moving distance
                        if (!moved && !withinAttackRange)
                        {
                            //set move
                            moveDecision = true;
                        }

                        //if it doesnt need to move to attack
                        if (withinAttackRange)
                        {
                            //go to action
                            actionState = State.Action;
                            //set up wait time
                            waitTimer = Time.time + attackWaitTime;
                            //set up action taken
                            actionComplete = false;
                            break;
                        }

                        if (!moved && moveDecision)
                        {
                            //movement decision
                            movePoints = EnemyLibrary.GetPossibleMovementPoints(enemyType, GetMatrixPos());
                            
                            //if there are valid movement points
                            if (movePoints.Length > 0)
                            {
                                //store possible points
                                List<int> possiblePointsWithWeapon = new List<int>();
                                List<int> weaponChosenForPoint = new List<int>();

                                //if theres no weapon choices
                                if (weaponChoice.Count <= 0)
                                {
                                    //add all weapons to weapon choice list
                                    for (int x = 0; x < weapons.Length; x++) weaponChoice.Add(x);
                                }
                                
                                //go through movement points
                                for (int i = 0; i < movePoints.Length; i++)
                                {
                                    //go through weapon choices
                                    for (int w = 0; w < weaponChoice.Count; w++)
                                    {
                                        //check if within weapon range + aoe
                                        foreach (Vector2Int v in weapons[weaponChoice[w]].GetRangeTiles(movePoints[i]))
                                        {
                                            if (playerPos == v)
                                            {
                                                possiblePointsWithWeapon.Add(i);
                                                weaponChosenForPoint.Add(w);
                                                break;
                                            }

                                            foreach (Vector2Int t in weapons[weaponChoice[w]].GetAoeTiles(v, movePoints[i]))
                                                if (playerPos == t)
                                                {
                                                    possiblePointsWithWeapon.Add(i);
                                                    weaponChosenForPoint.Add(w);
                                                    break;
                                                }
                                        }
                                    }
                                }
                                
                                //points with weapons
                                if (possiblePointsWithWeapon.Count > 0)
                                {
                                    //pick random
                                    int rnd = Random.Range(0, possiblePointsWithWeapon.Count);
                                    pointPick = possiblePointsWithWeapon[rnd];
                                    weaponPick = weaponChosenForPoint[rnd];
                                }
                                //no points with weapons?
                                else
                                {
                                    //store highest dmg weapon
                                    weaponPick = 0;
                                    for (int i = 0; i < weaponChoice.Count; i++)
                                    {
                                        if (weapons[weaponPick].GetPotency().y > weapons[i].GetPotency().y)
                                        {
                                            weaponPick = i;
                                        }
                                    }
                                    //pick point using highest dmg weapon
                                    pointPick = 0;
                                    bool picked = false;
                                    //store possibilities
                                    List<int> possiblePoints = new List<int>();
                                    for (int i = 0; i < movePoints.Length; i++)
                                    {
                                        float possiblePointDistanceToPlayer = CalculateDistanceTo(movePoints[i], playerPos);
                                        float currentPointDistanceToPlayer = CalculateDistanceTo(movePoints[pointPick], playerPos);
                                        float weaponMinDistance = weapons[weaponPick].GetRange().x + weapons[weaponPick].GetAreaOfEffect().x;
                                        //closest distance
                                        if (currentPointDistanceToPlayer >= possiblePointDistanceToPlayer + weaponMinDistance)
                                        {
                                            //store point
                                            possiblePoints.Add(i);
                                            //point was picked
                                            picked = true;
                                        }
                                    }
                                    //have possible points
                                    if (picked)
                                    {
                                        for (int i = 0; i < possiblePoints.Count; i++)
                                        {
                                            if (CalculateDistanceTo(movePoints[pointPick], playerPos) >
                                                CalculateDistanceTo(movePoints[possiblePoints[i]], playerPos))
                                            {
                                                pointPick = i;
                                            }
                                        }
                                    }
                                    //if no point was picked
                                    else
                                    {
                                        //end turn
                                        actionState = State.TurnEnd;
                                    }
                                }

                                //ajust timing
                                wait = true;
                                waitTimer = Time.time + moveWaitTime;
                                //highlight cell
                                if (movePoints != null && movePoints.Length > 0)
                                {
                                    highlight.PlaceHighlight(movePoints[pointPick]);
                                }
                                //move
                                actionState = State.Move;
                                break;
                            }
                        }

                        //if cant move or attack then end turn
                        if (!moveDecision && !withinAttackRange) actionState = State.TurnEnd;
                        break;
                    }
                case State.Move:
                    {
                        //after delay
                        if (!wait)
                        {
                            //move towards the point
                            transform.position = Vector3.MoveTowards(transform.position,
                                   nextPoint,
                                   movementSpeed * Time.deltaTime);
                            //if at the point
                            if (transform.position == nextPoint)
                            {
                                //delay
                                waitTimer = Time.time + moveWaitTime;
                                wait = true;
                            }
                            //if at the last point
                            if (transform.position.x == movePoints[pointPick].x &&
                                transform.position.z == movePoints[pointPick].y)
                            {
                                //clear highlights
                                highlight.ClearHighlights();
                                //no more movement
                                moved = true;
                                //action
                                actionState = State.Wait;
                            }
                        }
                        //if delay has passed
                        else if (waitTimer < Time.time)
                        {
                            //gets the next move point
                            nextPoint = GetNextMovePoint();
                            //end delay
                            wait = false;
                        }
                        break;
                    }
                case State.Action:
                    {
                        if (!actionComplete)
                        {
                            //get player pos
                            Vector2Int playerPos = new Vector2Int(Mathf.FloorToInt(player.transform.position.x),
                                Mathf.FloorToInt(player.transform.position.z));
                            //pick highest damage weapon
                            if (!usingAoe)
                            {
                                for (int i = 0; i < weaponChoice.Count; i++)
                                {
                                    if (weapons[weaponChoice[i]].GetPotency().y > weapons[weaponChoice[0]].GetPotency().y)
                                    {
                                        //store in 0 index of weapon choice
                                        weaponChoice[0] = weaponChoice[i];
                                        break;
                                    }
                                }
                            }
                            //using aoe
                            else
                            {
                                for (int i = 0; i < aoePointsWeapon.Count; i++)
                                {
                                    if (weapons[aoePointsWeapon[0]].GetPotency().y < weapons[aoePointsWeapon[i]].GetPotency().y)
                                    {
                                        aoePointsWeapon[0] = aoePointsWeapon[i];
                                        aoePoint = aoePoints[i];
                                    }
                                }
                            }
                            //create highlights
                            Vector2Int[] attackTiles ;
                            if (usingAoe)
                            {
                                Vector2Int[] aoeTiles = weapons[aoePointsWeapon[0]].GetAoeTiles(aoePoint, GetMatrixPos());
                                if (aoeTiles != null)
                                {
                                    for (int i = 0; i < aoeTiles.Length; i++)
                                    {
                                        highlight.PlaceHighlight(aoeTiles[i],Color.red);
                                    }
                                }
                                //range tiles
                                attackTiles = weapons[aoePointsWeapon[0]].GetRangeTiles(GetMatrixPos());
                            }else attackTiles = weapons[weaponChoice[0]].GetRangeTiles(GetMatrixPos());

                            if (attackTiles != null)
                            {
                                for (int i = 0; i < attackTiles.Length; i++)
                                {
                                    highlight.PlaceHighlight(attackTiles[i]);
                                }
                            }

                            //roll
                            int roll;
                            int pointIndexChoice;

                            if (!usingAoe) pointIndexChoice = weaponChoice[0];
                            else pointIndexChoice = aoePointsWeapon[0];

                            float critMod=0.0f;
                            if (player.GetComponent<StatusEffect>())
                                if (player.GetComponent<StatusEffect>().GetEffectType() == StatusEffect.EffectType.ExposedToCrit)
                                    critMod = player.GetComponent<StatusEffect>().effectPotency/100;

                            roll = weapons[pointIndexChoice].RollForDamage(critMod);

                            //rotate
                            if (animationController!=null)
                            {
                                Vector3 point = new Vector3(player.transform.position.x,0,player.transform.position.z);
                                animationController.RotateToFace(point);
                            }
                            //attack
                            player.GetComponent<PlayerController>().stats.ModifyHealthBy(-roll);
                            //log
                            GlobalGameState.UpdateLog(string.Format("<color=yellow>{0}</color> attacked and dealt <color=red>{1}</color> damage.",
                                name,roll));
                            //skill
                            if (weapons[pointIndexChoice].skill.skillType != Skills.SkillList.None)
                            {
                                //use skill
                                weapons[pointIndexChoice].skill.UseSkill(weapons[pointIndexChoice],null, player.GetComponent<PlayerControllerCombat>(),roll, gameObject);
                                //log
                                GlobalGameState.UpdateLog(string.Format("<color=yellow>{0}</color> used <color=purple>{1}</color>.",
                                    name, weapons[pointIndexChoice].skill.skillType));
                            }
                            //take ap
                            if(!usingAoe) stats.ModifyActionPointsBy(-weapons[weaponChoice[0]].getCost());
                            else stats.ModifyActionPointsBy(-weapons[aoePointsWeapon[0]].getCost());
                            //setup for wait
                            actionComplete = true;
                        }
                        //delay
                        if (waitTimer < Time.time)
                        {
                            //clear highlights
                            highlight.ClearHighlights();
                            //back to wait
                            actionState = State.Wait;
                        }
                        break;
                    }
                case State.TurnEnd:
                    {
                        //end turn pipeline
                        EndTurn();
                        break;
                    }
            }
        }
        if (actionState == State.Dead)
        {
            Destroy(gameObject, 1);
            if (!usedDeathParticles)
            {
                usedDeathParticles = true;
                Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);

                //add currency
                int currencyAdded=GameData.ModifyCurrencyBy(EnemyLibrary.getEnemyValue(enemyType));
                GlobalGameState.UpdateLog(string.Format("You've received {0} souls.", currencyAdded));

                //Drop Object
                
                GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<ItemDrop>().chanceToDrop();
            }
        }
    }

    private float CalculateDistanceTo(Vector3 me,Vector3 target) 
    {
        float dx = target.x - me.x;
        float dy = target.z - me.z;
        float sqrx = Mathf.Pow(dx, 2);
        float sqry = Mathf.Pow(dy, 2);
        float distanceToTarget = Mathf.Sqrt(sqrx) + Mathf.Sqrt(sqry);

        return distanceToTarget;
    }
    private float CalculateDistanceTo(Vector2Int me, Vector2Int target)
    {
        float dx = target.x - me.x;
        float dy = target.y - me.y;
        float sqrx = Mathf.Pow(dx, 2);
        float sqry = Mathf.Pow(dy, 2);
        float distanceToTarget = Mathf.Sqrt(sqrx) + Mathf.Sqrt(sqry);

        return distanceToTarget;
    }
    private Vector3 GetNextMovePoint() 
    {
        //get the last point
        Vector3 lastPoint = new Vector3(movePoints[pointPick].x, 0, movePoints[pointPick].y);
        //if not a horse
        if (enemyType != EnemyLibrary.EnemyType.Horse)
        {
            //get the next point with vector length of 1
            Vector3 point = transform.position + (lastPoint - transform.position).normalized;
            //round the point to the nearest integer, in case of diagonal movement
            point = new Vector3(Mathf.RoundToInt(point.x), 0, Mathf.RoundToInt(point.z));

            return point;
        }
        //if it is a horse
        else return lastPoint;
    }

    private bool canUseWeapon() 
    {
        bool canUse = true;
        foreach (Weapon w in weapons)
            if (w.getCost() > stats.GetCurrentActionPoints()) canUse = false;
        return canUse;
    }

    public void CombatStart() 
    {
        actionState = State.Wait;
        myTurn = false;
    }

    public void MyTurn()
    {
        GlobalGameState.UpdateLog(string.Format("{0}'s turn.", name));
        if (actionState == State.Dead)
        {
            EndTurn();
            return;
        }
        //stats and action points
        if (stats!=null)
            //refill ap
            stats.RefillActionPoints();
        else
            stats = EnemyLibrary.GetEnemyStats(enemyType);
        //set camera
        GameObject.FindGameObjectWithTag("FreeCam").GetComponent<CamMovement>().newTarget(gameObject);
        //move reset
        moved = false;
        //action reset
        actionComplete = false;
        //weapon pick reset
        weaponPick = -1;
        //aoe setup
        if (aoePoints != null) aoePoints.Clear();
        else aoePoints = new List<Vector2Int>();
        if (aoePointsWeapon != null) aoePointsWeapon.Clear();
        else aoePointsWeapon = new List<int>();
        //set initial state
        actionState = State.Wait;
        //tick effects
        if (GetComponent<StatusEffect>())
        {
            foreach (StatusEffect effect in GetComponents<StatusEffect>())
                effect.TurnTick();
        }
        //play
        myTurn = true;
    }

    public void EndTurn()
    {
        actionState = State.Idle;
        myTurn = false;
        CombatHandler.NextCombatantTurn();
    }

    private Vector2Int GetMatrixPos() 
    {
        return new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
    }

    public Stats GetStats() 
    {
        return stats;
    }

    public int ModifyHealthBy(int amount)
    {
        int value= stats.ModifyHealthBy(amount);
        return value;
    }

    public void Die() 
    {
        stats.ModifyHealthBy(-stats.GetCurrentHealth());
    }
}
