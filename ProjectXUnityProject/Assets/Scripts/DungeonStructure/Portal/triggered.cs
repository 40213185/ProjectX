using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class triggered : MonoBehaviour
{
    private float delay;
    public bool trig { get; private set; }

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        trig = false;
        delay = 3.0f+Time.time;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Time.time > delay)
            {
                trig = true;
                Debug.Log("Activated");
                delay = Time.time + 3.0f;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trig = false;
            Debug.Log("Deactivated");
        }
    }
}
