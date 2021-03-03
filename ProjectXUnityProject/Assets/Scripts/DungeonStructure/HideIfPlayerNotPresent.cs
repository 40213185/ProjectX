using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class HideIfPlayerNotPresent : MonoBehaviour
{
    private GameObject player;
    public GameObject hideThis;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hideThis.SetActive(false);
        GetComponent<BoxCollider>().isTrigger = true;
    }
    private void FixedUpdate()
    {
        if (hideThis.activeSelf)
        {
            GetComponent<BoxCollider>().enabled = false;
            enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.tag);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.transform.tag+" Enter");
        if (other.gameObject.transform.tag == "Player")
        {
            hideThis.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.transform.tag);
        if (other.gameObject.transform.tag == "Player")
        {
            hideThis.SetActive(true);
        }
    }
}
