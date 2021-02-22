using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapHandler
{
    public static Vector2[,] mapMatrix { get; set; }
    //the tyletype enumeration defines the int values of the matrixes
    //0-empty 1-walkable 2-obstacle
    public enum TileType { Empty, Walkable, Obstacle }

    //store room matrixes here
    //these must coincide with "MapGenerator" room prefabs
    private static List<int[,]> roomMatrixes=new List<int[,]>();

    static MapHandler() 
    {
        roomMatrixes.Add(new int[25, 25]{
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
    public static Vector2Int[,] GenerateMapMatrix(bool[,] roomPositionValidation,int[,] roomType)
    {
        return null;
    }
    public static Vector2Int GetTile(Vector2Int position)
    {
        return Vector2Int.zero;
    }
    public static Vector2Int[] GetMoveToPoints(Vector2Int initialPosition, Vector2Int finalPosition)
    {
        return null;
    }
    public static TileType GetTileTypeFromMatrix(Vector2Int matrixPosition)
    {
        return TileType.Empty;
    }
    public static TileType GetTileTypeFromPosition(Vector2Int position)
    {
        return TileType.Empty;
    }
}
