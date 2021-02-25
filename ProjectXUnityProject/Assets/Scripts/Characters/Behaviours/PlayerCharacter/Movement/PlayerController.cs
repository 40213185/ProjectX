using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum ControllerState 
    {
        Move,
        SelectAction,
        UseAction,
        Wait
    }
    public ControllerState controllerState;
    public float movementSpeed;
    private Vector2Int[] moveToPoints;
    public int movementRange;


    private Vector3 mouseClickPos;
    private int moveIndex;
    private float feetpos;
    private bool freeMove;

    // Start is called before the first frame update
    void Start()
    {
        controllerState = ControllerState.Wait;
        GlobalGameState.SetCombatState(false);

        mouseClickPos = new Vector3();
        feetpos = 0.5f;
        moveIndex = 0;
        freeMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (controllerState) 
        {
            case ControllerState.Wait:
                {
                    //replace code below with map vector points
                    if (Input.GetMouseButtonDown(0))
                    {
                        //disabel free move
                        freeMove = false;
                        //set up ray
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        //create a plane at floor level
                        Plane hitPlane = new Plane(Vector3.up, new Vector3(0,0,0));
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
                                Debug.Log(clickpos.ToString());
                                Debug.Log(MapHandler.GetTileTypeFromMatrix(clickpos).ToString());
                                if (MapHandler.GetTileTypeFromMatrix(clickpos) == MapHandler.TileType.Walkable)
                                {
                                    //get the movetopoints
                                    moveToPoints = MapHandler.GetMoveToPoints(initpos, clickpos,movementRange);
                                    //change state
                                    if (GlobalGameState.combatState == GlobalGameState.CombatState.Combat) controllerState = ControllerState.Move;
                                    else if (GlobalGameState.combatState == GlobalGameState.CombatState.OutOfCombat)
                                    {
                                        //adjust click position
                                        mouseClickPos = new Vector3(clickpos.x,feetpos,clickpos.y);

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

                    //free move if necessary
                    if (freeMove) 
                    {
                        //move prediction
                        Vector3 moveprediction = Vector3.MoveTowards(transform.position, mouseClickPos, movementSpeed * Time.deltaTime);
                        //get current tiletype for move prediction
                        if (MapHandler.GetTileTypeFromMatrix(new Vector2Int(Mathf.FloorToInt(moveprediction.x),Mathf.FloorToInt(moveprediction.z))) == MapHandler.TileType.Walkable)
                            //move if walkable
                            transform.position = moveprediction;
                        //else stop moving
                        else freeMove = false;
                        //arrived at position
                        if (transform.position == mouseClickPos) freeMove = false;
                    }
                    break;
            }
            case ControllerState.Move:
                {
                    if(MoveToPoint()) controllerState = ControllerState.Wait;
                    break;
                }
        }
    }

    private bool MoveToPoint()
    {
        if (moveIndex < moveToPoints.Length)
        {
            Vector3 movePoint = new Vector3(moveToPoints[moveIndex].x, feetpos, moveToPoints[moveIndex].y);
            transform.position = Vector3.MoveTowards(transform.position, movePoint, movementSpeed * Time.deltaTime);
            if (transform.position == movePoint) moveIndex++;
            return false;
        }
        else
        {
            moveIndex = 0;
            return true;
        }
    }
}
