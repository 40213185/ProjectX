using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //stats
    Stats stats;
    //camera
    public Camera controllerCamera;
    //highlighting cells
    private HighlightCells highlight;

    public enum State {
    Idle,
    Wait,
    Move,
    Action,
    TurnEnd
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

    //equipment
    private Weapon[] weapons;
    List<int> weaponChoice;
    bool withinAttackRange;
    private bool actionComplete;

    //player reference
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //actionState = State.Idle;
        stats = EnemyLibrary.GetEnemyStats(enemyType);

        player = GameObject.FindGameObjectWithTag("Player");

        waitTimer = Time.time + moveWaitTime;
        wait = false;

        //weapons
        weapons = EnemyLibrary.GetEnemyWeaponry(enemyType);
        weaponChoice = new List<int>();
        withinAttackRange = false;

        //highlighting
        highlight = HighlightCells.instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myTurn)
        {
            switch (actionState)
            {
                case State.Wait:
                    {
                        //setup for weapon decision
                        weaponChoice.Clear();

                        float distanceToPlayer = CalculateDistanceTo(transform.position,player.transform.position);
                        bool moveDecision = false;
                        withinAttackRange = false;
                        for (int i = 0; i < weapons.Length; i++)
                        {
                            //check if within weapon range + aoe
                            if ((distanceToPlayer - Mathf.FloorToInt(weapons[i].GetRange().y) - Mathf.FloorToInt(weapons[i].GetAreaOfEffect().y) <= 0)&&
                                (distanceToPlayer - Mathf.FloorToInt(weapons[i].GetRange().x) - Mathf.FloorToInt(weapons[i].GetAreaOfEffect().x) >= 0))
                            {
                                //add weapon possibility to list
                                weaponChoice.Add(i);
                                //set to attack
                                withinAttackRange = true;
                            }
                            //if above fails check with moving distance
                            else if (!moved && distanceToPlayer - Mathf.FloorToInt(weapons[i].GetRange().y) - Mathf.FloorToInt(weapons[i].GetAreaOfEffect().y) - GetStats().GetMaxMovementPoints() <= 0)
                            {
                                //add weapon possibility to list
                                weaponChoice.Add(i);
                                //set move
                                moveDecision = true;
                            }
                            //if above fails
                            else
                            {
                                //move
                                if (!moved) moveDecision = true;
                                else moveDecision = false;
                            }
                        }
                        //if it doesnt need to move to attack
                        if (withinAttackRange && !moveDecision)
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
                                //create player vector2Int reference
                                Vector2Int playerPos = new Vector2Int(Mathf.FloorToInt(player.transform.position.x),
                                    Mathf.FloorToInt(player.transform.position.z));

                                //store possible points
                                List<int> possiblePointsWithWeapon = new List<int>();
                                List<int> weaponChosenForPoint = new List<int>();
                                List<int> possiblePointsWithoutWeapon = new List<int>();

                                //pick movement point
                                for (int i = 0; i < movePoints.Length; i++)
                                {
                                    //if theres no weapon choices
                                    if (weaponChoice.Count <= 0)
                                    {
                                        //add all weapons to weapon choice list
                                        for (int x = 0; x < weapons.Length; x++) weaponChoice.Add(x);
                                    }

                                    //go though weapon choices
                                    for (int w = 0; w < weaponChoice.Count; w++)
                                    {
                                        //if within range of weapon
                                        distanceToPlayer = CalculateDistanceTo(movePoints[i], playerPos);
                                        if ((distanceToPlayer - Mathf.FloorToInt(weapons[weaponChoice[w]].GetRange().y) - Mathf.FloorToInt(weapons[weaponChoice[w]].GetAreaOfEffect().y) <= 0) &&
                                 (distanceToPlayer - Mathf.FloorToInt(weapons[weaponChoice[w]].GetRange().x) - Mathf.FloorToInt(weapons[weaponChoice[w]].GetAreaOfEffect().x) >= 0))
                                        {
                                            possiblePointsWithWeapon.Add(i);
                                            weaponChosenForPoint.Add(w);
                                        }
                                        //if not within weapon range
                                        else
                                        {
                                            possiblePointsWithoutWeapon.Add(i);
                                        }
                                    }
                                }
                                //have possible points with weapons?
                                if (possiblePointsWithWeapon.Count > 0)
                                {
                                    //reset pointpick
                                    pointPick = 0;
                                    weaponPick = 0;
                                    //go through points
                                    for (int i = 0; i < possiblePointsWithWeapon.Count; i++)
                                    {
                                        float possiblePointDistanceToPlayer = CalculateDistanceTo(movePoints[possiblePointsWithWeapon[i]], playerPos);
                                        float currentPointDistanceToPlayer = CalculateDistanceTo(movePoints[pointPick], playerPos);
                                        float weaponMinDistance = weapons[weaponChosenForPoint[i]].GetRange().x + weapons[weaponChosenForPoint[i]].GetAreaOfEffect().x;
                                        float weaponMaxDistance = weapons[weaponChosenForPoint[i]].GetRange().y + weapons[weaponChosenForPoint[i]].GetAreaOfEffect().y;
                                        //within range
                                        if ((currentPointDistanceToPlayer - possiblePointDistanceToPlayer + weaponMinDistance >= 0) &&
                                            (currentPointDistanceToPlayer - possiblePointDistanceToPlayer + weaponMaxDistance <= 0))
                                        /*
                                        //highest dmg weapon
                                        if ((pointDistanceToPlayer < currentDistanceToPlayer) &&
                                        (currentDistanceToPlayer >= pointDistanceToPlayer + weaponMinDistance))*/
                                        {
                                            pointPick = possiblePointsWithWeapon[i];
                                            weaponPick = weaponChosenForPoint[i];
                                        }
                                    }
                                }
                                //no points with weapons?
                                else if (possiblePointsWithoutWeapon.Count > 0)
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
                                    for (int i = 0; i < possiblePointsWithoutWeapon.Count; i++)
                                    {
                                        float possiblePointDistanceToPlayer = CalculateDistanceTo(movePoints[possiblePointsWithoutWeapon[i]], playerPos);
                                        float currentPointDistanceToPlayer = CalculateDistanceTo(movePoints[pointPick], playerPos);
                                        float weaponMinDistance = weapons[weaponPick].GetRange().x + weapons[weaponPick].GetAreaOfEffect().x;
                                        //closest distance
                                        if ( currentPointDistanceToPlayer >= possiblePointDistanceToPlayer + weaponMinDistance) 
                                        {
                                            //store point
                                            possiblePoints.Add(possiblePointsWithoutWeapon[i]);
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
                                                pointPick = possiblePoints[i];
                                            }
                                        }
                                    }
                                    //if no point was picked
                                    else
                                    {
                                        //pick random
                                        pointPick = possiblePointsWithoutWeapon[Random.Range(0, possiblePointsWithoutWeapon.Count)];
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
                        //already moved before?
                        else
                        {
                            actionState = State.TurnEnd;
                            break;
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
                            //create highlights
                            for (int x = -(int)(weapons[weaponChoice[0]].GetRange().y + weapons[weaponChoice[0]].GetAreaOfEffect().y)
                                ; x <= (int)(weapons[weaponChoice[0]].GetRange().y + weapons[weaponChoice[0]].GetAreaOfEffect().y)
                                ; x++)
                            {
                                for (int y = -(int)(weapons[weaponChoice[0]].GetRange().y + weapons[weaponChoice[0]].GetAreaOfEffect().y)
                                  ; y <= (int)(weapons[weaponChoice[0]].GetRange().y + weapons[weaponChoice[0]].GetAreaOfEffect().y)
                                  ; y++)
                                {
                                    Vector2Int checkPos = new Vector2Int(x, y)+GetMatrixPos();
                                    if((CalculateDistanceTo(GetMatrixPos(),checkPos)-weapons[weaponChoice[0]].GetRange().x-weapons[weaponChoice[0]].GetAreaOfEffect().x>=0)&&
                                        (CalculateDistanceTo(GetMatrixPos(), checkPos) - weapons[weaponChoice[0]].GetRange().y - weapons[weaponChoice[0]].GetAreaOfEffect().y <= 0))
                                        highlight.PlaceHighlight(checkPos);
                                }
                            }
                            //roll
                            int roll = weapons[weaponChoice[0]].RollForDamage();
                            //attack
                            player.GetComponent<PlayerController>().stats.ModifyHealthBy(-roll);
                            //setup for wait
                            actionComplete = true;
                            //take out times it can attack

                        }
                        //delay
                        if (waitTimer < Time.time)
                        {
                            //clear highlights
                            highlight.ClearHighlights();
                            //back to wait
                            actionState = State.Wait;
                            //end for now
                            //actionState = State.TurnEnd;
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

    public void CombatStart() 
    {
        actionState = State.Wait;
        myTurn = false;
    }

    public void MyTurn()
    {
        //set camera
        controllerCamera.enabled = true;
        //move reset
        moved = false;
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
        controllerCamera.enabled = false;
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
}
