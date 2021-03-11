using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControllerCombat))]

public class PlayerController : MonoBehaviour
{
    //stats
    private Stats stats;

    //camera
    public Camera controllerCamera;

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

    // Start is called before the first frame update
    void Start()
    {
        stats = new Stats(10, 3);                   //initial player stats
        combatController = GetComponent<PlayerControllerCombat>();
        combatController.enabled = false;

        controllerState = ControllerState.FreeMovement;

        mouseClickPos = new Vector3();
        feetpos = 0;
        move = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (controllerState)
        {
            //disable any kind of interaction until conditions are met
            case ControllerState.Freeze:
                {
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
                        //reset mouseclick
                        move = true;
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
                            Vector2Int clickpos = new Vector2Int(Mathf.FloorToInt(mouseClickPos.x), Mathf.FloorToInt(mouseClickPos.z));
                            try
                            {
                                if (MapHandler.GetTileTypeFromMatrix(clickpos) == MapHandler.TileType.Walkable)
                                {
                                    //adjust click position
                                    mouseClickPos = new Vector3(clickpos.x, feetpos, clickpos.y);
                                }
                            }
                            catch
                            {
                                Debug.Log("Index out of bounds error.");
                            }
                        }

                    }
                    if (move)
                    {
                        //move prediction
                        Vector3 moveprediction = Vector3.MoveTowards(transform.position, mouseClickPos+new Vector3(0,feetpos,0), movementSpeed * Time.deltaTime);
                        //get current tiletype for move prediction
                        if (MapHandler.GetTileTypeFromMatrix(new Vector2Int(Mathf.FloorToInt(moveprediction.x), Mathf.FloorToInt(moveprediction.z))) == MapHandler.TileType.Walkable&&
                            MapHandler.GetTileTypeFromMatrix(new Vector2Int(Mathf.CeilToInt(moveprediction.x), Mathf.CeilToInt(moveprediction.z))) == MapHandler.TileType.Walkable)
                            //move if walkable
                            transform.position = moveprediction;
                        //arrived at position or new click
                        if (transform.position == mouseClickPos) move = false ; //stop moving

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
        transform.position = new Vector3(Mathf.Floor(transform.position.x),feetpos,Mathf.FloorToInt(transform.position.z));

        //swap controllers
        combatController.enabled = true;
        enabled = false;
    }

    public void OnEnable()
    {
        controllerCamera.enabled = true;
    }

    public Stats GetStats() 
    {
        return stats;
    }
}