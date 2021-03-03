using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //stats
    Stats stats;

    public enum CombatControllerState
    {
        CombatMove,
        SelectAction,
        UseAction,
        Wait, //this state also handles out of combat behavior
        Freeze
    }
    //set to public for testing -  reset to private after testing cycle done
    public CombatControllerState combatControllerState;
    public float movementSpeed;
    public float outofcombatMovementSpeed;
    private Vector2Int[] moveToPoints;
    public int movementRange;

    //mostly movement things
    private Vector3 mouseClickPos;
    private int moveIndex;
    private float feetpos;
    //free movement is handled outside of controller state
    //while the state is in wait
    private bool freeMove;

    //combat
    private bool myTurn;

    // Start is called before the first frame update
    void Start()
    {
        stats = new Stats(10, 3);

        combatControllerState = CombatControllerState.Wait;
        GlobalGameState.SetCombatState(false);

        mouseClickPos = new Vector3();
        feetpos = 0.5f;
        moveIndex = 0;
        freeMove = false;
        MyTurn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //only perform actions if players turn
        //this is always set to true outside of combat
        if (myTurn)
        {
            switch (combatControllerState)
            {
                //disable any kind of interaction until conditions are met
                case CombatControllerState.Freeze:
                    {
                        break;
                    }

                case CombatControllerState.Wait:
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            //disable free move
                            freeMove = false;
                            //set up ray
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            //create a plane at floor level
                            Plane hitPlane = new Plane(Vector3.up, new Vector3(0, 0, 0));
                            //Plane.Raycast stores the distance from ray.origin to the hit point in this variable
                            float distance = 0;
                            //if the ray hits the plane
                            if (hitPlane.Raycast(ray, out distance))
                            {
                                //get the hit point
                                mouseClickPos = ray.GetPoint(distance);

                                //setup for maphandler functions
                                Vector2Int initpos = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
                                Vector2Int clickpos = new Vector2Int(Mathf.FloorToInt(mouseClickPos.x), Mathf.FloorToInt(mouseClickPos.z));
                                try
                                {
                                    if (MapHandler.GetTileTypeFromMatrix(clickpos) == MapHandler.TileType.Walkable)
                                    {
                                        //get the movetopoints
                                        moveToPoints = MapHandler.GetMoveToPoints(initpos, clickpos, movementRange);
                                        //change state
                                        if (GlobalGameState.combatState == GlobalGameState.CombatState.Combat && myTurn) combatControllerState = CombatControllerState.CombatMove;
                                        else if (GlobalGameState.combatState == GlobalGameState.CombatState.OutOfCombat)
                                        {
                                            //adjust click position
                                            mouseClickPos = new Vector3(clickpos.x, feetpos, clickpos.y);
                                            //free movement handled inside state, below
                                            freeMove = true;
                                        }
                                    }
                                }
                                catch
                                {
                                    Debug.Log("Index out of bounds error.");
                                }
                            }

                        }

                        //free move if out of combat
                        if (freeMove)
                        {
                            //move prediction
                            Vector3 moveprediction = Vector3.MoveTowards(transform.position, mouseClickPos, outofcombatMovementSpeed * Time.deltaTime);
                            //get current tiletype for move prediction
                            if (MapHandler.GetTileTypeFromMatrix(new Vector2Int(Mathf.FloorToInt(moveprediction.x+(moveprediction.normalized.x)), Mathf.FloorToInt(moveprediction.z+moveprediction.normalized.z))) == MapHandler.TileType.Walkable)
                                //move if walkable
                                transform.position = moveprediction;
                            //else stop moving
                            else freeMove = false;
                            //arrived at position
                            if (transform.position == mouseClickPos) freeMove = false; //stop moving
                        }
                        break;
                    }

                //combat movement
                case CombatControllerState.CombatMove:
                    {
                        if (MoveToPoint()) combatControllerState = CombatControllerState.Wait;
                        break;
                    }
            }
        }
    }

    private bool MoveToPoint()
    {
        //if it hasnt reached the array point limit + 1
        if (moveIndex < moveToPoints.Length)
        {
            //get the next move point
            Vector3 movePoint = new Vector3(moveToPoints[moveIndex].x, feetpos, moveToPoints[moveIndex].y);
            //move towards that point
            transform.position = Vector3.MoveTowards(transform.position, movePoint, movementSpeed * Time.deltaTime);
            //once reached go to next index
            if (transform.position == movePoint) moveIndex++;
            //not reached last point return false
            return false;
        }
        //over the array limit
        else
        {
            //reset index for next movement
            moveIndex = 0;
            //return finished
            return true;
        }
    }

    /// <summary>
    /// set to bool value. true=player able to move during combat, false=movement disabled
    /// </summary>
    /// <param name="value"></param>
    public void CombatStartPhase()
    {
        //disable all actions
        combatControllerState = CombatControllerState.Freeze;
        myTurn = false;
        CombatHandler.StartCombat(gameObject);
    }

    public void MyTurn() 
    {
        myTurn = true;
    }

    public void EndTurn() 
    {
        myTurn = false;
    }
}