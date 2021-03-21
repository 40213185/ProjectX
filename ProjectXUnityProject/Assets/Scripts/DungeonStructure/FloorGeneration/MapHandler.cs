using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapHandler
{
    public static int[,] mapMatrix { get; set; }

    //the tyletype enumeration defines the int values of the matrixes
    //0-empty 1-walkable 2-obstacle
    public enum TileType { Empty, Walkable, Obstacle,
        None }//keep none at the end of the enumeration

    //store room matrixes here
    //these must coincide with "MapGenerator" room prefabs
    private static List<int[,]> roomMatrixes = new List<int[,]>();

    public const int roomSizex = 25;
    public const int roomSizey = 25;

    static MapHandler()
    {
        //room 00
        roomMatrixes.Add(new int[roomSizex, roomSizey]{
        { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }});
        //room 01
        roomMatrixes.Add(new int[roomSizex, roomSizey]{
        { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }});
        //room 02
        roomMatrixes.Add(new int[roomSizex, roomSizey]{
        { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,2,2,2,2,2,2,2,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,2,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,2,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,2,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,2,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,2,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,2,2,2,2,2,2,2,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }});
        //room 03
        roomMatrixes.Add(new int[roomSizex, roomSizey]{
        { 2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }});
        //room 04
        roomMatrixes.Add(new int[roomSizex, roomSizey]{
        { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0 },
        { 2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }});
    }

    /// <summary>
    /// room position validation is an array of bool that has true on the array positions the room were set in
    /// room type should be according to MapGenerator prefab list and MapHandler matrix list, correspondingly
    /// </summary>
    /// <param name="roomPositionValidation"></param>
    /// <param name="roomType"></param>
    /// <returns></returns>
    public static int[,] GenerateMapMatrix(bool[,] roomPositionValidation, int[,] roomType)
    {
        //initialize map matrix
        mapMatrix = new int[roomPositionValidation.GetLength(0) * roomSizex, roomPositionValidation.GetLength(1) * roomSizey];
        //fill with 0's
        for (int x = 0; x < roomPositionValidation.GetLength(0); x++)
        {
            for (int y = 0; y < roomPositionValidation.GetLength(1); y++)
            {
                mapMatrix[x, y] = 0;
            }
        }

        //go through matrix
        for (int x = 0; x < roomPositionValidation.GetLength(0); x++)
        {
            for (int y = 0; y < roomPositionValidation.GetLength(1); y++)
            {
                //if it finds a room
                if (roomPositionValidation[x, y] == true)
                {
                    //pointer for room matrix
                    Vector2Int pointer = new Vector2Int(0, 0);
                    //fill with appropriate data
                    for (int yy = y * roomSizey; yy < (y * roomSizey) + roomSizey; yy++)
                    {
                        for (int xx = x * roomSizex; xx < (x * roomSizex) + roomSizex; xx++)
                        {
                            mapMatrix[xx, yy] = roomMatrixes[roomType[x, y]][pointer.y, pointer.x];

                            //increment pointer x
                            pointer.x++;
                        }
                        //increment pointer y
                        pointer.y++;
                        //reset pointer y
                        pointer.x = 0;
                    }
                }
            }
        }
        return mapMatrix;
    }
    public static Vector3 GetTilePosition(Vector3 position)
    {
        //convert position to int
        Vector3Int tilepos = new Vector3Int(Mathf.FloorToInt(position.x), 0, Mathf.FloorToInt(position.z));

        return tilepos;
    }

    public static TileType GetTileTypeFromMatrix(Vector2Int matrixPosition)
    {
        try
        {
            switch (mapMatrix[matrixPosition.x, matrixPosition.y])
            {
                case 0: return TileType.Empty;
                case 1: return TileType.Walkable;
                case 2: return TileType.Obstacle;
                default: return TileType.None;
            }
        }
        catch 
        {
            return TileType.Obstacle;
        }
    }
    public static TileType GetTileTypeFromPosition(Vector3 position)
    {
        Vector2Int vec2int = new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.z));
        return GetTileTypeFromMatrix(vec2int);
    }

    public static Vector2Int[] GetMoveToPoints(Vector2Int initialPosition, Vector2Int finalPosition, int range)
    {
        //start node
        Node startNode = new Node(initialPosition.x, initialPosition.y);
        //end node
        Node endNode = new Node(finalPosition.x, finalPosition.y);

        //open list with starting point
        List<Node> openList = new List<Node>() { startNode };
        //closed list
        List<Node> closedList = new List<Node>();

        //start node gcost
        startNode.gcost = 0;
        startNode.hcost = Node.CalculateDistance(initialPosition, finalPosition);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            //current node
            Node currentNode = Node.GetLowestFCostNode(openList);
           
            //remove from open
            openList.Remove(currentNode);
            //add to closed
            closedList.Add(currentNode);

            //go through neighbours
            foreach (Node n in Node.GetNeighbours(currentNode,initialPosition,finalPosition))
            {
                //if above range ignore (added padding)
                if (n.gcost > range * 1.5f) continue;

                //if its not a valid position
                if (!isValideMovementPosition(n)) continue;

                //check if its already in closed list
                for (int i = 0; i < closedList.Count; i++)
                {
                    if (closedList[i].getPosition()==n.getPosition()) continue;
                }

                //check cost of lowest cost neighbor plus cost to move (1 tile)
                float tentativeGcost = currentNode.gcost+Node.CalculateDistance(n.getPosition(),currentNode.getPosition());

                //if cost is lower or the same
                if (tentativeGcost <= n.gcost)
                {
                    //check if not in open list
                    bool found = false;
                    int index = 0;
                    for (int i = 0; i < openList.Count; i++)
                    {
                        //if found
                        if (openList[i].getPosition() == n.getPosition())
                        {
                            found = true;
                            index = i;
                        }
                    }

                    //if not found in the open list
                    if (!found)
                    {
                        //set data
                        n.parentNode = currentNode;
                        n.gcost = tentativeGcost;
                        n.CalculateHCost(n.getPosition(), finalPosition);
                        n.CalculateFCost();
                        //add
                        openList.Add(n);

                        //check if its the final node
                        if (openList[openList.Count-1].getPosition() == endNode.getPosition())
                            return CalculatedPath(openList);
                    }
                    //if found
                    else 
                    {
                        //replace data
                        openList[index].parentNode = currentNode;
                        openList[index].gcost = tentativeGcost;
                        openList[index].CalculateHCost(n.getPosition(), finalPosition);
                        openList[index].CalculateFCost();

                        //check if its the final node
                        if (openList[index].getPosition() == endNode.getPosition())
                            return CalculatedPath(openList);
                    }

                }
            }
        }
        //out of nodes?
        //return null
        return null;
    }

    private static bool isValideMovementPosition(Node currentNode) 
    {
        bool valid = true;
        if (GetTileTypeFromMatrix(currentNode.getPosition()) != TileType.Walkable) valid = false;

        if (GlobalGameState.combatState == GlobalGameState.CombatState.Combat)
        {
            foreach (GameObject obj in CombatHandler._combatants)
                if (obj.transform.position.x == currentNode.getPosition().x &&
                    obj.transform.position.z == currentNode.getPosition().y) valid = false;
        }
        return valid;
    }

    private static Vector2Int[] CalculatedPath(List<Node> nodeOpenList)
    {
        List<Node> path = new List<Node>();
        path.Add(nodeOpenList[nodeOpenList.Count-1]);
        Node current = path[0];
        while (current.parentNode != null)
        {
            path.Add(current.parentNode);
            if (path[path.Count - 1].getPosition() == nodeOpenList[0].getPosition()) break;
            current = current.parentNode;
        }
        path.Reverse();

        List<Vector2Int> points = new List<Vector2Int>();
        for (int i = 0; i < path.Count; i++)
        {
            points.Add(path[i].getPosition());
        }
        return points.ToArray();
    }
}

public class Node
{
    private int x;
    private int y;

    public float gcost;
    public float hcost;
    public float fcost;

    public Node parentNode;

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public Vector2Int getPosition() 
    {
        return new Vector2Int(x, y);
    }
    public void CalculateGCost(Vector2Int current, Vector2Int initial)
    {
        gcost = CalculateDistance(current,initial);
    }
    public void CalculateHCost(Vector2Int current, Vector2Int final)
    {
        hcost = CalculateDistance(current,final);
    }
    public void CalculateFCost()
    {
        fcost = gcost + hcost;
    }
    public static float CalculateDistance(Vector2Int a, Vector2Int b)
    {
        float dx = b.x - a.x;
        float dy = b.y - a.y;
        float sqrx = Mathf.Pow(dx, 2);
        float sqry = Mathf.Pow(dy, 2);
        float rootDistance = Mathf.Sqrt(sqrx) + Mathf.Sqrt(sqry);
        return rootDistance;
    }

    public static Node GetLowestFCostNode(List<Node> pathNodeList)
    {
        Node lowestFCostNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fcost < lowestFCostNode.fcost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    public static List<Node> GetNeighbours(Node currentNode,Vector2Int inititalPosition,Vector2Int finalPosition)
    {
        List<Node> neighbours = new List<Node>();
        neighbours.Add(new Node(currentNode.x + 1, currentNode.y));
        neighbours.Add(new Node(currentNode.x - 1, currentNode.y));
        neighbours.Add(new Node(currentNode.x, currentNode.y + 1));
        neighbours.Add(new Node(currentNode.x, currentNode.y - 1));
        //added diagonal movement outside of combat
        if (GlobalGameState.combatState == GlobalGameState.CombatState.OutOfCombat)
        {
            neighbours.Add(new Node(currentNode.x + 1, currentNode.y + 1));
            neighbours.Add(new Node(currentNode.x - 1, currentNode.y - 1));
            neighbours.Add(new Node(currentNode.x - 1, currentNode.y + 1));
            neighbours.Add(new Node(currentNode.x + 1, currentNode.y - 1));
        }

        foreach (Node n in neighbours)
        {
            n.CalculateGCost(n.getPosition(),inititalPosition);
            n.CalculateHCost(n.getPosition(),finalPosition);
            n.CalculateFCost();
        }



        return neighbours;
    }
}