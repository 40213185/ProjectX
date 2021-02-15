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
    private Vector3[] moveToPoints;

    //temp vars for testing
    private Vector3 mouseClickPos;
    private bool move;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        controllerState = ControllerState.Wait;

        move = false;
        cam = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (controllerState) 
        {
            case ControllerState.Wait:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        mouseClickPos = cam.ScreenToWorldPoint(Input.mousePosition);
                        mouseClickPos.x = Mathf.FloorToInt(mouseClickPos.x);
                        mouseClickPos.z = Mathf.FloorToInt(mouseClickPos.z);

                            move = true;
                            controllerState = ControllerState.Move;
                            moveToPoints = new Vector3[1] { new Vector3(mouseClickPos.x,0,mouseClickPos.z) };
                    }

                    break;
            }
            case ControllerState.Move:
                {
                    if (move) MoveToPoint();
                    if (transform.position == moveToPoints[0]) controllerState = ControllerState.Wait;
                    break;
                }
        }

    }

    private void MoveToPoint() 
    {
        Debug.Log(string.Format("moveToPos:{0}",moveToPoints[0]));
       transform.position = Vector3.MoveTowards(transform.position,moveToPoints[0], movementSpeed * Time.deltaTime);
    }
}
