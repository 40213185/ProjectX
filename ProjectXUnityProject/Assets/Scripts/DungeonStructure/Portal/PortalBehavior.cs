using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{
    public GameObject trigger;
    private GameObject teleportPosObject;
    private bool activated;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void FixedUpdate()
    {
        if (GlobalGameState.combatState == GlobalGameState.CombatState.Combat)
        {
            trigger.GetComponent<triggered>().ResetTrigger();
            if (trigger.activeSelf) trigger.SetActive(false);
        }
        else
        {
            if (activated && !trigger.activeSelf) trigger.SetActive(true);

            //if the trigger is active
            if (activated)
            {
                //and its triggered
                if (trigger.GetComponent<triggered>().trig)
                {
                    //disable next collider
                    teleportPosObject.GetComponent<PortalBehavior>().trigger.GetComponent<triggered>().DisableCollider();
                    //stop player movement
                    player.GetComponent<PlayerController>().StopMovement();
                    //teleport
                    player.transform.position = new Vector3(
                        teleportPosObject.transform.position.x,
                        player.GetComponent<PlayerController>().feetpos
                        , teleportPosObject.transform.position.z); ;
                }

                //player out of portal?
                if ((transform.position - player.transform.position).magnitude >= 1)
                    //reenable collider
                    trigger.GetComponent<triggered>().EnableCollider();
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