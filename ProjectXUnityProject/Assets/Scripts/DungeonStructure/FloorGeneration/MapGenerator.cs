using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //room matrix = 25x25
    //rooms should be lesser than 21x21 to accomodate hallway padding
    //Room prefab List(Update this as needed):
    //Room00: 20x20 floor
    public GameObject[] RoomPrefabs;
    public int[] RoomsPerFloor;
    private bool[,] roomsBuiltValidation;
    private int[,] roomType;

    private void GenerateMap(int roomsOnFloor)
    {
        //room build validation
        //this is initialized as false and set to true when a room is placed
        //used to check and avoid room overlapping
        roomsBuiltValidation = new bool[roomsOnFloor, roomsOnFloor];
        for (int x = 0; x < roomsOnFloor; x++)
        {
            for (int y = 0; y < RoomPrefabs.Length; y++)
            {
                roomsBuiltValidation[x, y] = false;
            }
        }
        //room type storage for use with MapHandler class
        roomType = new int[roomsOnFloor,roomsOnFloor];
        //random pointer for placing rooms properly
        Vector2Int pointer = new Vector2Int(0, 0);
        //cycle the amount of rooms fabricated at the current floor
        for (int i = 0; i < roomsOnFloor; i++)
        {
            //check if room placed
            if (!roomsBuiltValidation[pointer.x, pointer.y])
            {
                //pick a room
                int pick = Random.Range(0, RoomPrefabs.Length);
                Instantiate(RoomPrefabs[pick], new Vector3(pointer.x * MapHandler.roomSizex, 0, pointer.y * MapHandler.roomSizey), RoomPrefabs[pick].transform.rotation);
                roomsBuiltValidation[pointer.x, pointer.y] = true;
                roomType[pointer.x, pointer.y] = pick;
            }
            //create possible room placement array
            List<Vector2Int> possiblePlacements = new List<Vector2Int>();
            //populate list
            for (int x = 0; x < roomsOnFloor; x++)
            {
                for (int y = 0; y < roomsOnFloor; y++)
                {
                    //if theres no room placed
                    if (!roomsBuiltValidation[x, y])
                    {
                        //and theres a connection from below or the left 
                        if ((y - 1 >= 0 && roomsBuiltValidation[x, y - 1]) || (x - 1 >= 0 && roomsBuiltValidation[x - 1, y]))
                        {
                            //add to list
                            possiblePlacements.Add(new Vector2Int(x, y));
                        }
                    }
                }
            }
            //pick a random from the list and set it for new room placement
            pointer = possiblePlacements[Random.Range(0, possiblePlacements.Count - 1)];
        }
    }



    private void GenerateMapMatrix(int roomsOnFloor) 
    {
        MapHandler.GenerateMapMatrix(roomsBuiltValidation, roomType);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameData.SetFloor(4);
        //rooms per floor on current floor
        int roomsOnFloor = RoomsPerFloor[GameData.CurrentFloor];
        GenerateMap(roomsOnFloor);
        foreach (int i in roomType) Debug.Log(i.ToString() + " , ");
        GenerateMapMatrix(roomsOnFloor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
