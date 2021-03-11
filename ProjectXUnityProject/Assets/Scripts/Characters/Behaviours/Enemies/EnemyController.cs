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
                        //see if can attack
                        //decide which attack is best
                        //check if player is within range
                        //and go to action
                        //if not within range
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
                                //change so that it accomodates range
                                //and also checks if the current position is better than any of the
                                //possible movement points
                                //choose new point if the distance to the player is smaller
                                if ((movePoints[i] - playerPos).magnitude < (movePoints[pointPick] - playerPos).magnitude)
                                    pointPick = i;
                            }
                            //check if it should go to end of turn if its in the same position
                            //as the target position
                            //ajust timing
                            wait = true;
                            waitTimer = Time.time + waitTime;
                            //move
                            actionState = State.Move;
                        }
                        //end turn if no points to move to (in case of piece blocked)
                        else 
                        {
                            //check if within range of attack now
                            //go to action
                            //end turn
                            actionState = State.TurnEnd;
                        }
                        break;
                    }
                case State.Move:
                    {
                        if (!wait)
                        {
                            transform.position = Vector3.MoveTowards(transform.position,
                                   nextPoint,
                                   movementSpeed * Time.deltaTime);
                            if (transform.position == nextPoint)
                            {
                                waitTimer = Time.time + waitTime;
                                wait = true;
                            }
                            if (transform.position.x == movePoints[pointPick].x &&
                                transform.position.z == movePoints[pointPick].y)
                                actionState = State.TurnEnd;
                        }
                        else if (waitTimer < Time.time)
                        {
                            nextPoint = GetNextMovePoint();
                            wait = false;
                        }
                        break;
                    }
                case State.Action:
                    {
                        //attack
                        //take out times it can attack
                        //back to wait
                        break;
                    }
                case State.TurnEnd: 
                    {
                        EndTurn();
                        break;
                    }
            }
        }
    }

    private Vector3 GetNextMovePoint() 
    {
        Vector3 lastPoint = new Vector3(movePoints[pointPick].x, 0, movePoints[pointPick].y);
        Vector3 point = transform.position+(lastPoint-transform.position).normalized;
        point = new Vector3(Mathf.RoundToInt(point.x), 0, Mathf.RoundToInt(point.z));

        Debug.Log(string.Format("{0}->{1}->{2}",transform.position,point,lastPoint));
        
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
