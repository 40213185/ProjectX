using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerCombat : MonoBehaviour
{
    private Stats stats;
    //camera
    public Camera controllerCamera;
    //highlighting cells
    private HighlightCells highlight;

    public enum CombatControllerState
    {
        CombatMove,
        SelectAction,
        UseAction,
        Wait,
        EndTurn,
        Freeze
    }
    //set to public for testing -  reset to private after testing cycle done
    public CombatControllerState combatControllerState;
    public float movementSpeed;
    private Vector2Int[] moveToPoints;
    public int movementRange;

    //mostly movement things
    private Vector3 mouseClickPos;
    private Vector2Int initialPos;
    private int moveIndex;
    private float feetpos;
    public float waitTime;
    private float waitTimer;
    private bool wait;

    //combat
    private bool myTurn;

    //actions
    private Weapon weaponSelected;
    private List<GameObject> combatantsInfluenceByAction;
    private bool actionComplete;

    // Start is called before the first frame update
    void Start()
    {
        stats = gameObject.GetComponent<PlayerController>().stats;
        combatControllerState = CombatControllerState.Freeze;

        mouseClickPos = new Vector3();
        feetpos = 0;
        moveIndex = 0;

        waitTimer = Time.time + waitTime;
        wait = false;

        //highlighting
        highlight = HighlightCells.instance;

        //actions
        combatantsInfluenceByAction = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //speed up in combat
        if (Input.GetButton("Jump"))
            Time.timeScale = 4;
        else if (Time.timeScale > 1)
            Time.timeScale = 1;

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
                            if (UIBlock()) break;
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
                                Vector2Int initpos = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
                                Vector2Int clickpos = new Vector2Int(Mathf.FloorToInt(mouseClickPos.x), Mathf.FloorToInt(mouseClickPos.z));
                                initialPos = initpos;

                                if (CanMove(clickpos)&&stats.GetCurrentMovementPoints()>0)
                                {
                                    if (MapHandler.GetTileTypeFromMatrix(clickpos) == MapHandler.TileType.Walkable)
                                    {
                                        //get the movetopoints
                                        moveToPoints = MapHandler.GetMoveToPoints(initpos, clickpos, movementRange);
                                        //change state
                                        if (GlobalGameState.combatState == GlobalGameState.CombatState.Combat && myTurn && moveToPoints != null)
                                        {
                                            //reset index for next movement
                                            moveIndex = 0;
                                            //move
                                            combatControllerState = CombatControllerState.CombatMove;
                                            //highlight cell
                                            if (moveToPoints != null && moveToPoints.Length > 0)
                                            {
                                                for (int i = 0; i < moveToPoints.Length; i++)
                                                {
                                                    highlight.PlaceHighlight(moveToPoints[i]);
                                                    if (Node.CalculateDistance(initialPos, moveToPoints[i]) >= stats.GetCurrentMovementPoints()) break;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }

                        break;
                    }

                //combat movement
                case CombatControllerState.CombatMove:
                    {
                        if (MoveToPoint())
                        {
                            //round transform position
                            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
                                feetpos,
                                Mathf.RoundToInt(transform.position.z));
                            //clear highlights
                            highlight.ClearHighlights();
                            //change state on end movement
                            combatControllerState = CombatControllerState.Wait;
                        }
                        break;
                    }

                case CombatControllerState.SelectAction:
                    {
                        //if (waitTimer <= Time.time)
                        //{
                            if (UIBlock()) break;
                            if (Input.GetMouseButtonDown(0))
                            {
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
                                    Vector2Int[] selectionTiles = weaponSelected.GetRangeTiles(getMatrixPos());
                                    Debug.Log(clickpos.ToString());
                                    //check for outside range click
                                    bool outsideRange = true;
                                //go through range tiles
                                for (int i = 0; i < selectionTiles.Length; i++)
                                {
                                    //if the tile is at click pos
                                    if (selectionTiles[i] == clickpos)
                                    {
                                        Debug.Log("Click in range");
                                        outsideRange = false;
                                        //clear highlights
                                        highlight.ClearHighlights();
                                        //go through aoetiles at click pos
                                        Vector2Int[] aoeTiles = weaponSelected.GetAoeTiles(selectionTiles[i], getMatrixPos());
                                        for (int z = 0; z < aoeTiles.Length; z++)
                                        {
                                            //place aoe highlights
                                            highlight.PlaceHighlight(aoeTiles[z]);
                                            //check against combatants in area
                                            for (int c = 0; c < CombatHandler._combatants.Length; c++)
                                            {
                                                Vector2Int curcombatantMatrixPos =
                                                    new Vector2Int(Mathf.FloorToInt(CombatHandler._combatants[c].transform.position.x),
                                                    Mathf.FloorToInt(CombatHandler._combatants[c].transform.position.z));
                                                //add to list if combatant within the area but not self
                                                if (curcombatantMatrixPos != getMatrixPos() &&
                                                    curcombatantMatrixPos == aoeTiles[z])
                                                {
                                                    bool add = true;
                                                    //go through combatants influenced list
                                                    for (int ciba = 0; ciba < combatantsInfluenceByAction.Count; ciba++)
                                                    {
                                                        //and only add if not added already
                                                        if (combatantsInfluenceByAction[ciba] != CombatHandler._combatants[c])
                                                        {
                                                            add = false;
                                                            break;
                                                        }
                                                    }
                                                    if(add) combatantsInfluenceByAction.Add(CombatHandler._combatants[c]);
                                                }
                                            }
                                        }
                                        if (combatantsInfluenceByAction != null && combatantsInfluenceByAction.Count > 0)
                                        {
                                            //take ap
                                            stats.ModifyActionPointsBy(-weaponSelected.getCost());
                                            //set up wait
                                            waitTimer = Time.time + waitTime;
                                            //setup action used once
                                            actionComplete = false;
                                            //use action
                                            combatControllerState = CombatControllerState.UseAction;
                                        }
                                        break;
                                    }
                                }

                                    //go back to wait state if clicked outside range
                                    if (outsideRange)
                                    {
                                        highlight.ClearHighlights();
                                        combatControllerState = CombatControllerState.Wait;
                                    }
                                }
                            }
                        //}
                        break;
                    }
                case CombatControllerState.UseAction:
                    {
                        if (!actionComplete)
                        {
                            //use weapon on every combatant
                            for (int i = 0; i < combatantsInfluenceByAction.Count; i++)
                            {
                                if (combatantsInfluenceByAction[i].GetComponent<EnemyController>())
                                {
                                    int amount = combatantsInfluenceByAction[i].GetComponent<EnemyController>().ModifyHealthBy(-weaponSelected.RollForDamage());
                                    Debug.Log("Dealt " + amount);
                                }
                            }
                            actionComplete = true;
                        }else if (waitTimer <= Time.time)
                        {
                            //clear highlights
                            highlight.ClearHighlights();
                            combatControllerState = CombatControllerState.Wait;
                        }
                        break;
                    }
                //end turn
                case CombatControllerState.EndTurn:
                    {
                        EndTurn();
                        break;
                    }
            }
        }
    }

    private bool UIBlock() 
    {
        bool blocked = false;
        //block on ui hit
#if !UNITY_EDITOR

                        if (EventSystem.current.IsPointerOverGameObject(0)) blocked=true;
#else
        if (EventSystem.current.IsPointerOverGameObject(-1)) blocked = true; ;
#endif
        return blocked;
    }

    private bool CanMove(Vector2Int clickpos)
    {
        //check enemy position
        bool canmove = true;
        for (int i = 0; i < CombatHandler._combatants.Length; i++)
        {
            if (CombatHandler._combatants[i].tag != "Player")
            {
                //setup vector2int for comparison
                Vector2Int enemyPlacement = new Vector2Int(Mathf.FloorToInt(CombatHandler._combatants[i].transform.position.x),
                    Mathf.FloorToInt(CombatHandler._combatants[i].transform.position.z));
                //compare and break if necessary
                if (enemyPlacement == clickpos)
                {
                    canmove = false;
                    break;
                }
            }
        }
        return canmove;
    }

    private bool MoveToPoint()
    {
        //if it hasnt reached the array point limit
        if (moveIndex < moveToPoints.Length)
        {
            if (!wait)
            {
                //get the next move point
                Vector3 movePoint = new Vector3(moveToPoints[moveIndex].x, feetpos, moveToPoints[moveIndex].y);
                //tile not occupied?
                if (CanMove(new Vector2Int(Mathf.FloorToInt(movePoint.x), Mathf.FloorToInt(movePoint.z))))
                {
                    //move towards that point
                    transform.position = Vector3.MoveTowards(transform.position, movePoint, movementSpeed * Time.deltaTime);
                    //once reached go to next index
                    if (transform.position == movePoint)
                    {
                        moveIndex++;
                        wait = true;
                        waitTimer = Time.time + waitTime;
                        //check if the distance travelled is bigger or equal to movement points available
                        if (Node.CalculateDistance(initialPos, new Vector2Int((int)movePoint.x, (int)movePoint.z)) >= stats.GetCurrentMovementPoints())
                        {
                            //take mp
                            stats.ModifyMovementPointsBy(-(int)Node.CalculateDistance(initialPos, new Vector2Int((int)movePoint.x, (int)movePoint.z)));
                            GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateUI();
                            return true;
                        }
                    }
                    //not reached last point return false
                    return false;
                }
                else
                {
                    //get the previous move point
                    movePoint = new Vector3(moveToPoints[moveIndex - 1].x, feetpos, moveToPoints[moveIndex - 1].y);
                    //take mp
                    stats.ModifyMovementPointsBy(-(int)Node.CalculateDistance(initialPos, new Vector2Int((int)movePoint.x, (int)movePoint.z)));
                    GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateUI();
                    //reset index for next movement
                    moveIndex = 0;
                    //return finished
                    return true;
                }
            }
            else if (waitTimer < Time.time)
            {
                wait = false;
                return false;
            }
            else return false;
        }
        //over the array limit
        else
        {
            //reset index for next movement
            moveIndex = 0;
            //take mp
            stats.ModifyMovementPointsBy(-(int)Node.CalculateDistance(initialPos, new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z))));
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateUI();
            //return finished
            return true;
        }
    }

    public void MyTurn()
    {
        //ap
        stats.RefillActionPoints();
        //mp
        stats.RefillMovementPoints();
        //camera
        controllerCamera.enabled = true;
        //controller
        combatControllerState = CombatControllerState.Wait;
        //turn
        myTurn = true;
    }

    public void EndTurn()
    {
        controllerCamera.enabled = false;
        combatControllerState = CombatControllerState.Freeze;
        myTurn = false;
        CombatHandler.NextCombatantTurn();
    }

    public Vector2Int getMatrixPos()
    {
        return new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
    }

    public void SelectAction(Weapon w)
    {
        weaponSelected = w;
        //clear highlights
        highlight.ClearHighlights();
        //prep and place highlights
        Vector2Int[] rangeTiles = weaponSelected.GetRangeTiles(getMatrixPos());
        for (int i = 0; i < rangeTiles.Length; i++)
        {
            highlight.PlaceHighlight(rangeTiles[i]);
        }
        //change to wait for select action
        combatControllerState = CombatControllerState.SelectAction;
        //setup small delay for mouse intput delayed action
        waitTimer = Time.time + 1;
    }
}
