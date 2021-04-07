using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    //room matrix = 25x25
    //rooms should be lesser than 21x21 to accomodate hallway padding
    //Room prefab List(Update this as needed):
    //Room00: 20x20 floor
    //Room08: StartingRoom

    public GameObject[] RoomPrefabs;

    public enum RoomType { Room00, Room01, Room02, Room03, Room04, SmallCrossRoom, SmallRoom, SmallRoom02, StartRoom }
    
    public GameObject[] clutter00; 
    public GameObject[] clutter01;
    public GameObject[] clutter02;
    public GameObject[] clutter03;
    public GameObject[] clutter04;
    public GameObject[] clutterSCR;
    public GameObject[] clutterSR;
    public GameObject[] clutterSR02;
    public GameObject[] clutterStart;

    public int[] RoomsPerFloor;
    public bool[,] roomsBuiltValidation { get; private set; }
    private int[,] roomType;


    public GameObject player;
    public List<GameObject> rooms { get; private set; }

    public GameObject UI;

    private void GenerateMap(int roomsOnFloor)
    {
        //save rooms setup
        rooms = new List<GameObject>();
        //room build validation
        //this is initialized as false and set to true when a room is placed
        //used to check and avoid room overlapping
        roomsBuiltValidation = new bool[roomsOnFloor, roomsOnFloor];
        for (int y = 0; y < roomsOnFloor; y++)
        {
            for (int x = 0; x < roomsOnFloor; x++)
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
                int pick;
                if(i == 0) 
                {
                    pick = 8;
                }
                else 
                { 
                    pick = Random.Range(0, RoomPrefabs.Length-1); 
                }
                //pick a room
                
                //create room and store it
                rooms.Add(Instantiate(RoomPrefabs[pick], new Vector3(pointer.x * MapHandler.roomSizex, 0, pointer.y * MapHandler.roomSizey), RoomPrefabs[pick].transform.rotation));
                //set room built here
                roomsBuiltValidation[pointer.x, pointer.y] = true;
                //store room type
                roomType[pointer.x, pointer.y] = pick;
            }
            //create possible room placement array
            List<Vector2Int> possiblePlacements = new List<Vector2Int>();
            //populate list
            for (int y = 0; y < roomsOnFloor; y++)
            {
                for (int x = 0; x < roomsOnFloor; x++)
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

    private void PositionPlayer() 
    {
        Instantiate(player, new Vector3(3, 0, 1), player.transform.rotation);
    }

    private void PositionUI() 
    {
        Instantiate(UI, new Vector3(0, 0, 0), UI.transform.rotation);
    }

    private void ClutterPlacementStep(int x, int y,GameObject[] clutterChoice)
    {
        if (clutterChoice.Length > 0)
        {
            //pick random clutter
            int pick = Random.Range(0, clutterChoice.Length);

            //place clutter
            GameObject clutter = Instantiate(clutterChoice[pick], new Vector3(x * MapHandler.roomSizex, 0, y * MapHandler.roomSizey), clutterChoice[pick].transform.rotation);

            //find room
            for (int i = 0; i < rooms.Count; i++)
            {
                //found room
                if (rooms[i].transform.position.x == x * MapHandler.roomSizex && rooms[i].transform.position.z == y * MapHandler.roomSizey)
                {
                    //set to hide/show
                    rooms[i].GetComponent<HideIfPlayerNotPresent>().SetChild(clutter);
                }
            }

            //go through children and change map matrix
            for (int yy = y * MapHandler.roomSizex; yy < (y * MapHandler.roomSizey) + MapHandler.roomSizey; yy++)
            {
                for (int xx = x * MapHandler.roomSizex; xx < (x * MapHandler.roomSizex) + MapHandler.roomSizex; xx++)
                {
                    for (int i = 0; i < clutter.transform.childCount; i++)
                    {
                        //ignore if its partciles
                        if (clutter.transform.GetChild(i).GetComponent<ParticleSystem>()) continue;
                        //mesh size
                        float xsize = clutter.transform.GetChild(i).GetComponent<MeshFilter>().mesh.bounds.size.x;
                        if (xsize > 0.4f)
                        {
                            //vectors for mad handling
                            Vector3 childV3 = clutter.transform.GetChild(i).transform.position;
                            Vector2Int childV2Int = new Vector2Int(Mathf.FloorToInt(childV3.x), Mathf.FloorToInt(childV3.z));
                            //set origin tile to obstacle
                            if (MapHandler.GetTileTypeFromMatrix(childV2Int) == MapHandler.TileType.Walkable)
                                MapHandler.ConvertTileToType(childV2Int, MapHandler.TileType.Obstacle);
                            //set additional tile to obstacle
                            if (xsize > 1.0f)
                            {
                                //check which tile
                                if (clutter.transform.GetChild(i).transform.rotation.y != 0 ||
                                    clutter.transform.GetChild(i).transform.rotation.y != 180)
                                    childV2Int.y++;
                                else childV2Int.x++;

                                //set to obstacle
                                if (MapHandler.GetTileTypeFromMatrix(childV2Int) == MapHandler.TileType.Walkable)
                                    MapHandler.ConvertTileToType(childV2Int, MapHandler.TileType.Obstacle);
                            }
                        }
                    }
                }
            }
        }
    }

    private void PlaceClutter()
    {
        for (int y = 0; y < roomsBuiltValidation.GetLength(0); y++)
        {
            for (int x = 0; x < roomsBuiltValidation.GetLength(0); x++)
            {
                if (roomsBuiltValidation[x, y])
                {
                    switch (roomType[x, y])
                    {
                        case 0: { ClutterPlacementStep(x, y, clutter00); break; }
                        case 1: { ClutterPlacementStep(x, y, clutter01); break; }
                        case 2: { ClutterPlacementStep(x, y, clutter02); break; }
                        case 3: { ClutterPlacementStep(x, y, clutter03); break; }
                        case 4: { ClutterPlacementStep(x, y, clutter04); break; }
                        case 5: { ClutterPlacementStep(x, y, clutterSCR); break; }
                        case 6: { ClutterPlacementStep(x, y, clutterSR); break; }
                        case 7: { ClutterPlacementStep(x, y, clutterSR02); break; }
                        case 8: { ClutterPlacementStep(x, y, clutterStart); break; }
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize game data
        GameData.SetFloor(4);
        InventorySystem.init(4);

        //initialize map
        //rooms per floor on current floor
        int roomsOnFloor = RoomsPerFloor[GameData.CurrentFloor];
        GenerateMap(roomsOnFloor);
        GenerateMapMatrix(roomsOnFloor);
        PlaceClutter();
        PositionPlayer();
        PositionUI();
        GlobalGameState.SetCombatState(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
