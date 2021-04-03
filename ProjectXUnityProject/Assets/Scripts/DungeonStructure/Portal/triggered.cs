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

                delay = Time.time + 1.0f;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trig = false;
        }
    }
    public void ResetTrigger() {
        trig = false;
    }
}
