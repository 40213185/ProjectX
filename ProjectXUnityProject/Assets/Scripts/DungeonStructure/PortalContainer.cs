using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalContainer : MonoBehaviour
{
    public GameObject[] portalContainer;
    private bool showPortals;
    private GameObject mapGenerator;

    // Start is called before the first frame update
    void Start()
    {
        showPortals = false;
        SetPortals(showPortals);
        mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator");
    }

    private void SetPortals(bool active) 
    {
        foreach (GameObject go in portalContainer) go.GetComponent<PortalBehavior>().trigger.SetActive(active);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //time to show the portals?
        if (showPortals)
        {
            ShowAndConnectPortals(1, 0);
            ShowAndConnectPortals(0, 1);
            ShowAndConnectPortals(-1, 0);
            ShowAndConnectPortals(0, -1);

            enabled = false;
        }
    }

    private void ShowAndConnectPortals(int x, int y)
    {
        //check array bounds
        if ((x < mapGenerator.GetComponent<MapGenerator>().roomsBuiltValidation.GetLength(0) && x >= 0) &&
            (y < mapGenerator.GetComponent<MapGenerator>().roomsBuiltValidation.GetLength(1) && y >= 0))
        {
            Debug.Log("Valid index position");
            //check if theres a room using room validation in map generator
            if (mapGenerator.GetComponent<MapGenerator>().roomsBuiltValidation[Mathf.FloorToInt(transform.position.x / MapHandler.roomSizex) + x, Mathf.FloorToInt(transform.position.z / MapHandler.roomSizey) + y])
            {
                Debug.Log("Checking room at index position: " + (Mathf.FloorToInt(transform.position.x / MapHandler.roomSizex) + x).ToString() + " : " +
                    (Mathf.FloorToInt(transform.position.z / MapHandler.roomSizey) + y).ToString());
                //get room
                for (int i = 0; i < mapGenerator.GetComponent<MapGenerator>().rooms.Count; i++)
                {
                    //find the correct room?
                    if (mapGenerator.GetComponent<MapGenerator>().rooms[i].transform.position.x ==
                        transform.position.x + x * MapHandler.roomSizex &&
                       mapGenerator.GetComponent<MapGenerator>().rooms[i].transform.position.z ==
                        transform.position.z + y * MapHandler.roomSizey)
                    {
                        Debug.Log("RoomFound at " + mapGenerator.GetComponent<MapGenerator>().rooms[i].transform.position.ToString());

                        //get the portal closest to it
                        int closestPortalCurrent = 0;
                        //get the closest portal point on the other rooms portals
                        int closestPortalFurthest = 0;

                        //go through portals in this room
                        for (int thisRoomsPortal = 0; thisRoomsPortal < portalContainer.Length; thisRoomsPortal++)
                        {
                            //go through portals in next room
                            for (int otherRoomsPortal = 0;
                                otherRoomsPortal < mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().portalContainer.Length;
                                otherRoomsPortal++)
                            {
                                //check if that portal is already active
                                if (!portalContainer[thisRoomsPortal].GetComponent<PortalBehavior>().isActivate() &&
                                    !mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().portalContainer[otherRoomsPortal].GetComponent<PortalBehavior>().isActivate())
                                {
                                    //compare the distance between each to find the closest ones
                                    if ((mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().portalContainer[otherRoomsPortal].transform.position
                                        - portalContainer[thisRoomsPortal].transform.position).magnitude <
                                        (mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().portalContainer[closestPortalFurthest].transform.position
                                        - portalContainer[closestPortalCurrent].transform.position).magnitude)
                                    {
                                        closestPortalCurrent = thisRoomsPortal;
                                        closestPortalFurthest = otherRoomsPortal;
                                    }
                                }
                            }
                        }
                        //activate
                        portalContainer[closestPortalCurrent].GetComponent<PortalBehavior>().Activate();
                        mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().portalContainer[closestPortalFurthest].GetComponent<PortalBehavior>().Activate();
                        //set port position
                        portalContainer[closestPortalCurrent].GetComponent<PortalBehavior>().SetTeleportPos(
                            mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().portalContainer[closestPortalFurthest]);
                        mapGenerator.GetComponent<MapGenerator>().rooms[i].GetComponent<PortalContainer>().portalContainer[closestPortalFurthest].GetComponent<PortalBehavior>().SetTeleportPos(
                            portalContainer[closestPortalCurrent]);
                        Debug.Log("Portal activated");
                        break;
                    }
                }
            }
        }
    }

    public void ActivatePortalPipeline() 
    {
        showPortals = true;
    }
}
