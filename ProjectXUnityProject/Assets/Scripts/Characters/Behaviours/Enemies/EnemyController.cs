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
    public float waitTime;
    private float waitTimer;
    private bool wait;

    private Vector2Int[] movePoints;
    private int pointPick;
    private bool moved;

    //equipment
    private Weapon[] weapons;
    int weaponChoice;
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

        waitTimer = Time.time + waitTime;
        wait = false;

        //weapons
        weapons = EnemyLibrary.GetEnemyWeaponry(enemyType);
        weaponChoice = 0;
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
                        weaponChoice = 0;
                        float distanceToPlayer = (transform.position - player.transform.position).magnitude;
                        bool moveDecision = false;
                        for (int i = 0; i < weapons.Length; i++)
                        {
                            //check if within weapon range + aoe
                            if (distanceToPlayer - Mathf.FloorToInt(weapons[i].GetRange().magnitude) - weapons[i].GetAreaOfEffect().magnitude <= 0)
                            {
                                //check potencies
                                if (weapons[weaponChoice].GetPotency().magnitude < weapons[i].GetPotency().magnitude)
                                {
                                    weaponChoice = i;
                                }
                                //set to attack
                                withinAttackRange = true;
                            }
                            //if above fails check with moving distance
                            else if (distanceToPlayer - Mathf.FloorToInt(weapons[i].GetRange().magnitude) - Mathf.FloorToInt(weapons[i].GetAreaOfEffect().magnitude) - GetStats().GetMaxMovementPoints() <= 0)
                            {
                                //check potencies
                                if (weapons[weaponChoice].GetPotency().magnitude < weapons[i].GetPotency().magnitude)
                                {
                                    weaponChoice = i;
                                }
                                //set move
                                if(!moved) moveDecision = true;
                                //set to attack
                                withinAttackRange = true;
                            }
                            //if above fails
                            else 
                            {
                                //check distance to player is bigger or equal 1
                                if (distanceToPlayer >= 1) moveDecision = true;
                                else moveDecision = false;
                                withinAttackRange = false;
                            }
                        }
                        //if it doesnt need to move to attack
                        if (withinAttackRange && !moveDecision)
                        {
                            //go to action
                            actionState = State.Action;
                            //set up wait time
                            waitTimer = Time.time + waitTime;
                            //set up action taken
                            actionComplete = false;
                            break;
                        }

                        if (!moved)
                        {
                            if (moveDecision)
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
                                        //if player is within attack range
                                        if (withinAttackRange)
                                        {
                                            //choose new point based on distance to player + weapon range + weapon aoe
                                            if ((movePoints[i] - playerPos).magnitude -
                                                weapons[weaponChoice].GetRange().magnitude - weapons[weaponChoice].GetAreaOfEffect().magnitude <
                                                (movePoints[pointPick] - playerPos).magnitude -
                                                weapons[weaponChoice].GetRange().magnitude - weapons[weaponChoice].GetAreaOfEffect().magnitude)
                                                pointPick = i;
                                        }
                                        //choose new point if the distance to the player is smaller and player not within attack range
                                        else if ((movePoints[i] - playerPos).magnitude < (movePoints[pointPick] - playerPos).magnitude)
                                            pointPick = i;
                                    }
                                    //ajust timing
                                    wait = true;
                                    waitTimer = Time.time + waitTime;
                                    //move
                                    actionState = State.Move;
                                    break;
                                }
                            }
                            else actionState = State.TurnEnd;
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
                                waitTimer = Time.time + waitTime;
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
                            int roll = weapons[weaponChoice].RollForDamage();
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
        //get the next point with vector length of 1
        Vector3 point = transform.position+(lastPoint-transform.position).normalized;
        //round the point to the nearest integer, in case of diagonal movement
        point = new Vector3(Mathf.RoundToInt(point.x), 0, Mathf.RoundToInt(point.z));

        return point;
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
