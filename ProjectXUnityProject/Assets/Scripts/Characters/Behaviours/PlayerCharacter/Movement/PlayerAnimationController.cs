using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animationController;
    private Vector3 directionVector,previousPosition,newPosition;

    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }

    protected void Initialise()
    {
        previousPosition = transform.position;
        newPosition = transform.position;
    }
    protected void UpdateRotation()
    {
        //store new position
        newPosition = transform.position;

        //get the new direction vector
        directionVector = newPosition - previousPosition;

        if (directionVector.magnitude > 0)
            transform.rotation = Quaternion.Euler(0, -90 + Mathf.Atan2(directionVector.z, -directionVector.x) * Mathf.Rad2Deg, 0);


        //stores the old position
        previousPosition = newPosition;
    }
}
