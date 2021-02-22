using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum ControllerState 
    {
        FreeMovement,
        Move,
        SelectAction,
        UseAction,
        Wait
    }
    public ControllerState controllerState;
    public float movementSpeed;
    private Vector2Int[] moveToPoints;

    //temp vars for testing
    private Vector3 mouseClickPos;
    private int moveIndex;
    private float delay;
    private float feetpos;

    // Start is called before the first frame update
    void Start()
    {
        controllerState = ControllerState.Wait;
        GlobalGameState.SetCombatState(false);

        mouseClickPos = new Vector3();
        feetpos = 0.5f;
        moveIndex = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (controllerState) 
        {
            case ControllerState.Wait:
                {
                    //replace code below with map vector points
                    if (Input.GetMouseButtonDown(0))
                    {
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
                                if (MapHandler.GetTileTypeFromMatrix(clickpos) == MapHandler.TileType.Walkable)
                                {
                                    //get the movetopoints
                                    moveToPoints = MapHandler.GetMoveToPoints(initpos, clickpos);
                                    //change state
                                    if (GlobalGameState.combatState == GlobalGameState.CombatState.Combat) controllerState = ControllerState.Move;
                                    else if (GlobalGameState.combatState == GlobalGameState.CombatState.OutOfCombat)
                                    {
                                        mouseClickPos = new Vector3(clickpos.x,feetpos,clickpos.y);
                                        controllerState = ControllerState.FreeMovement;
                                    }
                                }
                            }
                            catch 
                            {
                                Debug.Log("Index out of bounds error.");
                            }
                        }

                    }
                    break;
            }
            case ControllerState.Move:
                {
                    if(MoveToPoint()) controllerState = ControllerState.Wait;
                    break;
                }
            case ControllerState.FreeMovement:
                {
                    //move to position
                    transform.position = Vector3.MoveTowards(transform.position, mouseClickPos, movementSpeed * Time.deltaTime);
                    //change state
                    if (transform.position == mouseClickPos) controllerState = ControllerState.Wait;
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
