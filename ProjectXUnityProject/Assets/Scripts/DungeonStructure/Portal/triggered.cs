using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class triggered : MonoBehaviour
{
    public bool trig { get; private set; }

    private void Start()
    {
        trig = false;
        GetComponent<BoxCollider>().isTrigger = true;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trig = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ResetTrigger();
        }
    }
    public void ResetTrigger()
    {
        trig = false;
    }
    public void DisableCollider() 
    {
        GetComponent<BoxCollider>().enabled = false;
    }
    public void EnableCollider() 
    {
        GetComponent<BoxCollider>().enabled = true;
    }
}