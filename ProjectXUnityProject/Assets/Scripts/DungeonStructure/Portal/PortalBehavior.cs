using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{
    public GameObject trigger;
    private GameObject teleportPosObject;
    private bool activated;
    private float timer, delay;

    private void Start()
    {
        delay = 3.0f;
    }

    public void setDelay()
    {
        timer = Time.time + delay;
    }

    public void FixedUpdate()
    {
        if (GlobalGameState.combatState == GlobalGameState.CombatState.Combat)
        {
            if (trigger.activeSelf) trigger.SetActive(false);
        }
        else
        {
            if (activated && !trigger.activeSelf) trigger.SetActive(true);

            //if the trigger is active
            if (activated)
            {
                //and its triggered
                if (trigger.GetComponent<triggered>().trig && Time.time > timer)
                {
                    //add delay to next portal so no insta back to portal
                    teleportPosObject.GetComponent<PortalBehavior>().setDelay();
                    //stop player movement
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().StopMovement();
                    //teleport
                    GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(
                        teleportPosObject.transform.position.x,
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().feetpos
                        , teleportPosObject.transform.position.z); ;
                }
            }
        }
    }

    public void SetTeleportPos(GameObject connectedPortal)
    {
        teleportPosObject = connectedPortal;
    }

    public void Activate()
    {
        activated = true;
    }
    public bool isActivate()
    {
        return activated;
    }
}