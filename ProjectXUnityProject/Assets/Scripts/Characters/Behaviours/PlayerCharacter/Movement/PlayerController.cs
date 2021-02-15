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
    private int moveIndex;
    private float delay;

    // Start is called before the first frame update
    void Start()
    {
        controllerState = ControllerState.Wait;

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
                        moveToPoints = new Vector3[3];
                        moveToPoints[0] = new Vector3(Random.Range(
                            10,
                            20),
                            1,
                            Random.Range(10,
                            20));
                        moveToPoints[1] = new Vector3(Random.Range(
                            moveToPoints[0].x + 10,
                            moveToPoints[0].x + 20),
                            1,
                            Random.Range(
                            moveToPoints[0].z + 10,
                            moveToPoints[0].z + 20));
                        moveToPoints[2] = new Vector3(Random.Range(
                            moveToPoints[1].x + 10,
                            moveToPoints[1].x + 20),
                            1,
                            Random.Range(
                            moveToPoints[1].z + 10,
                            moveToPoints[1].z + 20));

                        moveIndex = 0;
                        controllerState = ControllerState.Move;
                    }
                    break;
            }
            case ControllerState.Move:
                {
                    if(moveIndex<moveToPoints.Length) MoveToPoint();
                    else controllerState = ControllerState.Wait;
                    break;
                }
        }
    }

    private void MoveToPoint()
    {
        if (transform.position == moveToPoints[moveIndex])
        {
            moveIndex++;
            delay = Time.time + 0.5f;
        }
        if(Time.time>delay)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPoints[moveIndex], movementSpeed * Time.deltaTime);
        }

    }
}
