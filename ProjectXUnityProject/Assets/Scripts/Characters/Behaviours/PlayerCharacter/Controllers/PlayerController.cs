using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerControllerCombat))]

public class PlayerController : MonoBehaviour
{
    //stats
    public Stats stats { get; private set; }

    //combatController
    private PlayerControllerCombat combatController;

    public enum ControllerState
    {
        FreeMovement,
        Wait,
        Freeze
    }
    //set to public for testing -  reset to private after testing cycle done
    public ControllerState controllerState;
    public float movementSpeed;

    //mostly movement things
    private Vector3 mouseClickPos;
    public float feetpos { get; private set; }
    private bool move;  //used for checking movement during movement
    private Vector2Int[] moveToPoints;
    private int movePointsIndex;

    //highlighting cells
    private HighlightCells highlight;

    // Start is called before the first frame update
    void Start()
    {
        stats = GameData.stats;                 //initial player stats
        stats.RefillHealth();
        combatController = GetComponent<PlayerControllerCombat>();
        combatController.enabled = false;

        controllerState = ControllerState.FreeMovement;

        mouseClickPos = new Vector3();
        feetpos = 0;
        move = false;

        highlight = HighlightCells.instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (controllerState)
        {
            //disable any kind of interaction until conditions are met
            case ControllerState.Freeze:
                {
                    move = false;
                    if (GlobalGameState.combatState == GlobalGameState.CombatState.OutOfCombat)
                        controllerState = ControllerState.FreeMovement;
                    break;
                }

            case ControllerState.Wait:
                {
                    break;
                }
            case ControllerState.FreeMovement:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        //block on ui hit
#if !UNITY_EDITOR

                        if (EventSystem.current.IsPointerOverGameObject(0)) break;
#else
                        if (EventSystem.current.IsPointerOverGameObject(-1)) break;
#endif
                        //clear highlights if they exist;
                        highlight.ClearHighlights();
                        //set up ray
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        //create a plane at floor level
                        Plane hitPlane = new Plane(Vector3.up, new Vector3(0, -0.5f, 0));
                        //Plane.Raycast stores the distance from ray.origin to the hit point in this variable
                        float distance = 0;
                        //if the ray hits the plane
                        if (hitPlane.Raycast(ray, out distance))
                        {
                            //get the hit point
                            mouseClickPos = ray.GetPoint(distance);

                            //setup for maphandler functions
                            Vector2Int clickpos = new Vector2Int(Mathf.FloorToInt(mouseClickPos.x), Mathf.FloorToInt(mouseClickPos.z));
                            try
                            {
                                if (MapHandler.GetTileTypeFromMatrix(clickpos) == MapHandler.TileType.Walkable)
                                {
                                    //get the movetopoints
                                    moveToPoints = MapHandler.GetMoveToPoints(new Vector2Int(Mathf.FloorToInt(transform.position.x),
                                        Mathf.FloorToInt(transform.position.z)), clickpos, 100);
                                    //reset index
                                    movePointsIndex = 1;
                                    //reset mouseclick
                                    move = true;
                                    //highlight cell
                                    if (moveToPoints!=null&&moveToPoints.Length>0)
                                    {
                                        for (int i = 0; i < moveToPoints.Length; i++)
                                        {
                                            highlight.PlaceHighlight(moveToPoints[i]);
                                        }
                                    }
                                }
                            }
                            catch (UnityException e)
                            {
                                Debug.Log("Error: "+e.ToString());
                            }
                        }

                    }
                    if (move && moveToPoints != null)
                    {
                        //at the last point
                        if (transform.position.x == moveToPoints[moveToPoints.Length - 1].x &&
                            transform.position.z == moveToPoints[moveToPoints.Length - 1].y)
                        {
                            //clear highlights
                            highlight.ClearHighlights();
                            //stop movement
                            move = false;
                            break;
                        }
                        //not at the end yet?
                        else
                        {
                            //move towards point
                            transform.position = Vector3.MoveTowards(transform.position,
                            new Vector3(moveToPoints[movePointsIndex].x, feetpos, moveToPoints[movePointsIndex].y),
                            movementSpeed * Time.deltaTime);

                            //if at next point
                            if (transform.position.x == moveToPoints[movePointsIndex].x &&
                                transform.position.z == moveToPoints[movePointsIndex].y)
                            {
                                //increase index
                                movePointsIndex++;
                                //moveindex out of range?
                                if (movePointsIndex >= moveToPoints.Length)
                                {
                                    //clear highlights
                                    highlight.ClearHighlights();
                                    //stop moving
                                    move = false;
                                }
                            }
                        }
                    }
                    break;
                }
        }
    }


    /// <summary>
    /// set to bool value. true=player able to move during combat, false=movement disabled
    /// </summary>
    /// <param name="value"></param>
    public void CombatStartPhase()
    {
        //disable all actions
        controllerState = ControllerState.Freeze;

        //set player to correct cell
        transform.position = new Vector3(Mathf.Floor(transform.position.x), feetpos, Mathf.FloorToInt(transform.position.z));

        //swap controllers
        combatController.enabled = true;
        enabled = false;
    }

    public void CombatEndPhase() 
    {
        GlobalGameState.SetCombatState(false);
        //movement
        controllerState = ControllerState.Freeze;
        //if lost combat
        if (stats.GetCurrentHealth() <= 0) Debug.Log("Dead");
    }


    public void StopMovement()
    {
        move = false;
    }

    public Stats GetPlayerStatValues() 
    {
        return stats;
    }
}