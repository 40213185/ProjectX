using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class HideIfPlayerNotPresent : MonoBehaviour
{
    public GameObject hideThis;
    private List<GameObject> hideChildren;

    // Start is called before the first frame update
    void Start()
    {
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
            //spawn portals
            gameObject.GetComponent<PortalContainer>().ActivatePortalPipeline();
            //spawn if there is an enemy spawner
            if (GetComponent<EnemySpawner>()) GetComponent<EnemySpawner>().CheckAndPlaceEnemies();

            foreach (GameObject obj in hideChildren) obj.SetActive(true);
            hideThis.SetActive(true);
        }
    }

    public void SetChild(GameObject gamobj)
    {
        if (hideChildren == null) hideChildren = new List<GameObject>();
        hideChildren.Add(gamobj);
        hideChildren[hideChildren.Count - 1].SetActive(false);
    }
}
