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

    private void OnTriggerEnter(Collider other)
    {
        OnTriggeredByPlayer(other);
    }
    private void OnTriggerStay(Collider other)
    {
        //OnTriggeredByPlayer(other);
    }

    private void OnTriggeredByPlayer(Collider other) 
    {
        if (other.gameObject.transform.tag == "Player")
        {
            //spawn if there is an enemy spawner
            if (GetComponent<EnemySpawner>()) GetComponent<EnemySpawner>().CheckAndPlaceEnemies();

            hideThis.SetActive(true);
        }
    }
}
