using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapHandler
{
    public static int[,] mapMatrix { get; set; }

    //the tyletype enumeration defines the int values of the matrixes
    //0-empty 1-walkable 2-obstacle
    public enum TileType { Empty, Walkable, Obstacle ,
        None }//keep none at the end of the enumeration

    //store room matrixes here
    //these must coincide with "MapGenerator" room prefabs
    private static List<int[,]> roomMatrixes=new List<int[,]>();

    public const int roomSizex = 25;
    public const int roomSizey = 25;

    static MapHandler() 
    {
        roomMatrixes.Add(new int[roomSizex, roomSizey]{
        { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,0,0,0 },
        { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }} );
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
        mapMatrix = new int[roomPositionValidation.GetLength(0)*roomSizex, roomPositionValidation.GetLength(1)*roomSizey];
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
                    Vector2Int pointer = new Vector2Int(0,0);
                    //fill with appropriate data
                    for (int xx = x*roomSizex; xx < (x*roomSizex)+roomSizex; xx++)
                    {
                        for (int yy = y*roomSizey; yy < (y*roomSizey)+roomSizey; yy++)
                        {
                            switch (roomType[x, y]) 
                            {
                                case 0: {
                                        mapMatrix[xx, yy] = roomMatrixes[0][pointer.x,pointer.y];
                                        break;
                                    }
                            }
                            //increment pointer y
                            pointer.y++;
                        }
                        //increment pointer x
                        pointer.x++;
                        //reset pointer y
                        pointer.y = 0;
                    }
                }
            }
        }
        Debug.Log(mapMatrix.ToString());
        return mapMatrix;
    }
    public static Vector3 GetTilePosition(Vector3 position)
    {
        //convert position to int
        Vector3Int tilepos=new Vector3Int(Mathf.FloorToInt(position.x),0,Mathf.FloorToInt(position.z));

        return tilepos;
    }
    public static Vector2Int[] GetMoveToPoints(Vector2Int initialPosition, Vector2Int finalPosition)
    {
        return null;
    }
    public static TileType GetTileTypeFromMatrix(Vector2Int matrixPosition)
    {
        switch (mapMatrix[matrixPosition.x, matrixPosition.y])
        {
            case 0: return TileType.Empty;
            case 1: return TileType.Walkable;
            case 2: return TileType.Obstacle;
            default: return TileType.None;
        }
    }
    public static TileType GetTileTypeFromPosition(Vector3 position)
    {
        Vector2Int vec2int = new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.z));
        return GetTileTypeFromMatrix(vec2int);
    }
}
