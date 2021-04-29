using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class HideIfPlayerNotPresent : MonoBehaviour
{
    public GameObject hideThis;
    private List<GameObject> hideChildren;
    private bool triggered;
    private bool finished;

    // Start is called before the first frame update
    void Start()
    {
        hideThis.SetActive(false);
        GetComponent<BoxCollider>().isTrigger = true;
        triggered = false;
        finished = false;
    }
    private void FixedUpdate()
    {
        if (finished)
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

    private IEnumerator DelayForCombatStart() 
    {
        triggered = true;
        yield return new WaitForSecondsRealtime(1f);

        if (GetComponent<EnemySpawner>()) GetComponent<EnemySpawner>().CheckAndPlaceEnemies();
        else if (GetComponent<BossFightStart>()) GetComponent<BossFightStart>().StartFight();
        finished = true;
    }
    private void OnTriggeredByPlayer(Collider other) 
    {
        if (other.gameObject.transform.tag == "Player")
        {
            //spawn portals
            gameObject.GetComponent<PortalContainer>().ActivatePortalPipeline();
            //spawn if there is an enemy spawner
            if(!triggered) StartCoroutine("DelayForCombatStart");

            if(hideChildren!=null) foreach (GameObject obj in hideChildren) obj.SetActive(true);
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
