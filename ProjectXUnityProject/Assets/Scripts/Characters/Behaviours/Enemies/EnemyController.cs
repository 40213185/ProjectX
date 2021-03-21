using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //stats
    Stats stats;
    //camera
    public Camera controllerCamera;

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
                        float distanceToPlayer = (transform.position - player.transform.position).magnitude;
                        bool moveDecision = false;
                        withinAttackRange = false;
                        for (int i = 0; i < weapons.Length; i++)
                        {
                            //check if within weapon range + aoe
                            if (distanceToPlayer - Mathf.FloorToInt(weapons[i].GetRange().y) - Mathf.FloorToInt(weapons[i].GetAreaOfEffect().y) <= 0)
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
                                //check distance to player is bigger or equal 1
                                if (distanceToPlayer > 1 && !moved) moveDecision = true;
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
                                //reset pointpick
                                pointPick = 0;
                                //create player vector2Int reference
                                Vector2Int playerPos = new Vector2Int(Mathf.FloorToInt(player.transform.position.x),
                                    Mathf.FloorToInt(player.transform.position.z));
                                //pick movement point
                                for (int i = 0; i < movePoints.Length; i++)
                                {
                                    if (!withinAttackRange)
                                    {
                                        //if theres no weapon choices
                                        if (weaponChoice.Count <= 0)
                                        {
                                            //add all weapons to weapon choice list
                                            for (int x = 0; x < weapons.Length; x++) weaponChoice.Add(x);
                                        }
                                    }

                                    if (weaponChoice.Count > 1)
                                    {
                                        //pick weapon
                                        for (int weapPick = 0; weapPick < weaponChoice.Count; weapPick++)
                                        {
                                            //check if (min potency + max potency) / 2 is bigger than the weapon at 0 index
                                            if ((weapons[weaponChoice[0]].GetPotency().x + weapons[weaponChoice[0]].GetPotency().y) / 2 <
                                                (weapons[weaponChoice[weapPick]].GetPotency().x + weapons[weaponChoice[weapPick]].GetPotency().y) / 2)
                                            {
                                                //replace 0 index
                                                weaponChoice[0] = weapPick;
                                            }
                                        }
                                    }

                                    //if within range of weapon
                                    if ((new Vector2(movePoints[i].x, movePoints[i].y) + weapons[weaponChoice[0]].GetRange() + weapons[weaponChoice[0]].GetAreaOfEffect()).magnitude
                                        <= (movePoints[i] - playerPos).magnitude)
                                    {
                                        pointPick = i;
                                        break;
                                    }
                                    else
                                    {
                                        //choose new point based on distance to player + weapon range + weapon aoe
                                        if ((movePoints[i] - playerPos +
                                            weapons[weaponChoice[0]].GetRange() + weapons[weaponChoice[0]].GetAreaOfEffect()).magnitude <
                                            (movePoints[pointPick] - playerPos +
                                            weapons[weaponChoice[weaponChoice[0]]].GetRange() + weapons[weaponChoice[0]].GetAreaOfEffect()).magnitude)
                                            pointPick = i;
                                    }
                                }
                                //ajust timing
                                wait = true;
                                waitTimer = Time.time + moveWaitTime;
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
                            //back to wait
                            //actionState = State.Wait;
                            //end for now
                            actionState = State.TurnEnd;
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
