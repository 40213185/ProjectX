using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public bool following;
    public CharacterController cont;
    public Camera cam;
    private float camMoveSpeed;
    private float camZoomSpeed;

    private GameObject target;
    

    // Start is called before the first frame update
    void Start()
    {
        camMoveSpeed = 3;
        camZoomSpeed = 3;
        //following = true;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 camForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        Vector3 camRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z);

        //Toggle if folloing the Player (WILL NEED TO CHANGE THIS FOR DURING COMBAT, VITOR HELP ME WORK OUT HOW COMBAT WORKS PLEASE")
        if (Input.GetKeyDown("space")) 
        {
            following = !following;
            if (following) 
            {
                target = GameObject.FindGameObjectWithTag("Player");
                Debug.Log("Target =" + target.name);
            }
        }

        //Interpolate to the targets location for smooth follow, With no Interpolate looks jagged, Only do if following a target
        if (following) 
        {
            camPosUpdate(Vector3.Lerp(gameObject.transform.position, target.transform.position, 3 * Time.deltaTime));
        }


        //If the Player tries to manually control the camera, disable target follow and allow the player to control the camera
        if (following && x != 0 || following && z != 0)
        {
            following = false;
        }


        //Zoom in and Zoom out
        if (scroll != 0) 
        {
            if (scroll > 0 && cam.orthographicSize > 0 || scroll < 0 && cam.orthographicSize < 10) cam.orthographicSize -= (scroll * camZoomSpeed);
        }


        //Free Movement, Get the direction the camera is facing and move in the direction according to the camera and the keys pressed
        if (z != 0)
        {
            gameObject.transform.position += camForward * (z * camMoveSpeed * cam.orthographicSize) * Time.deltaTime;
        }

        if (x != 0) 
        {
            gameObject.transform.position += camRight * (x * camMoveSpeed * cam.orthographicSize) * Time.deltaTime;
        }
    }

    void camPosUpdate(Vector3 pos) 
    {
        gameObject.transform.position = pos;
    }
}
