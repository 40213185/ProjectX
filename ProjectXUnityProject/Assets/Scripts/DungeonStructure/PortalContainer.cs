using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HideIfPlayerNotPresent))]

public class PortalContainer : MonoBehaviour
{
    public GameObject[] portalContainer;
    private GameObject mapGenerator;
    private GameObject leftPortal, topPortal, rightPortal, bottomPortal;

    // Start is called before the first frame update
    void Start()
    {
        SetPortals(false);
        mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator");

        leftPortal = portalContainer[0];
        rightPortal = portalContainer[1];
        topPortal = portalContainer[2];
        bottomPortal = portalContainer[3];

        for (int i = 0; i < portalContainer.Length; i++)
        {
            for (int z = 0; z < portalContainer.Length; z++)
            {
                if (i==0&&leftPortal.transform.position.x > portalContainer[z].transform.position.x) leftPortal = portalContainer[z];
                if (i==1&&rightPortal.transform.position.x < portalContainer[z].transform.position.x) rightPortal = portalContainer[z];
                if (i==2&&topPortal.transform.position.z < portalContainer[z].transform.position.z) topPortal = portalContainer[z];
                if (i==3&&bottomPortal.transform.position.z > portalContainer[z].transform.position.z) bottomPortal = portalContainer[z];
            }
        }
    }

    private void SetPortals(bool active) 
    {
        foreach (GameObject go in portalContainer) go.GetComponent<PortalBehavior>().trigger.SetActive(active);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void ShowAndConnectPortals(int x, int y)
    {
        try
        {
            //check if theres a room using room validation in map generator
            if (mapGenerator.GetComponent<MapGenerator>().roomsBuiltValidation[Mathf.FloorToInt(transform.position.x / MapHandler.roomSizex) + x, Mathf.FloorToInt(transform.position.z / MapHandler.roomSizey) + y])
            {
                //get room
                for (int i = 0; i < mapGenerator.GetComponent<MapGenerator>().rooms.Count; i++)
                {
                    //find the correct room?
                    if (mapGenerator.GetComponent<MapGenerator>().rooms[i].transform.position.x ==
                        transform.position.x + x * MapHandler.roomSizex &&
                       mapGenerator.GetComponent<MapGenerator>().rooms[i].transform.position.z ==
                        transform.position.z + y * MapHandler.roomSizey)
                    {
                        //right portal
                        if (x == 1)
                        {
                            //activate
                            rightPortal.GetComponent<PortalBehavior>().Activate();
                            mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().leftPortal.GetComponent<PortalBehavior>().Activate();
                            //set port position
                            rightPortal.GetComponent<PortalBehavior>().SetTeleportPos(
                                mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().leftPortal);
                            mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().leftPortal.GetComponent<PortalBehavior>().SetTeleportPos(
                                rightPortal);
                        }
                        //left portal
                        else if (x == -1)
                        {
                            //activate
                            leftPortal.GetComponent<PortalBehavior>().Activate();
                            mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().rightPortal.GetComponent<PortalBehavior>().Activate();
                            //set port position
                            leftPortal.GetComponent<PortalBehavior>().SetTeleportPos(
                                mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().rightPortal);
                            mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().rightPortal.GetComponent<PortalBehavior>().SetTeleportPos(
                                leftPortal);
                        }
                        else {
                            //top portal
                            if (y == 1)
                            {
                                //activate
                                topPortal.GetComponent<PortalBehavior>().Activate();
                                mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().bottomPortal.GetComponent<PortalBehavior>().Activate();
                                //set port position
                                topPortal.GetComponent<PortalBehavior>().SetTeleportPos(
                                    mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().bottomPortal);
                                mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().bottomPortal.GetComponent<PortalBehavior>().SetTeleportPos(
                                    topPortal);
                            }
                            //bottom portal
                            else if (y == -1)
                            {
                                //activate
                                bottomPortal.GetComponent<PortalBehavior>().Activate();
                                mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().topPortal.GetComponent<PortalBehavior>().Activate();
                                //set port position
                                bottomPortal.GetComponent<PortalBehavior>().SetTeleportPos(
                                    mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().topPortal);
                                mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().topPortal.GetComponent<PortalBehavior>().SetTeleportPos(
                                    bottomPortal);
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            //Debug.Log("Portal Container invalid index");
        }
        
    }

    public void ActivatePortalPipeline()
    {
        ShowAndConnectPortals(1, 0);
        ShowAndConnectPortals(0, 1);
        ShowAndConnectPortals(-1, 0);
        ShowAndConnectPortals(0, -1);
    }
}
