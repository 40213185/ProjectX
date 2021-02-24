using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    public float deadZone;
    private Transform player;
    private Vector3 moveCast;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceToPlayer= (new Vector3(gameObject.transform.position.x,0,gameObject.transform.position.z) - new Vector3(player.position.x, 0, player.position.z)).magnitude;
        Debug.Log(distanceToPlayer.ToString());
            //sort target
            Vector3 playerBase = player.position;
            playerBase.y = 0;
            //sort movement cast
            moveCast = Vector3.MoveTowards(gameObject.transform.position, playerBase, cameraSpeed * Time.deltaTime);
            //move
            gameObject.transform.position += moveCast;
    }
}
