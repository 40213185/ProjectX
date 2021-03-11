using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyLibrary
{
    public enum EnemyType
    {
        Pawn,
        Rook,
        Bishop,
        Horse,
        King,
        Queen
    }

    public static Stats GetEnemyStats(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case (EnemyType.Pawn):
                {
                    return new Stats(5, 1);
                }
            case (EnemyType.Rook):
                {
                    return new Stats(20, 20);
                }
            case (EnemyType.Bishop):
                {
                    return new Stats(15, 20);
                }
            case (EnemyType.Horse):
                {
                    return new Stats(12, 3);
                }
            case (EnemyType.King):
                {
                    return new Stats(30, 1);
                }
            case (EnemyType.Queen):
                {
                    return new Stats(50, 20);
                }
            default:
                {
                    return new Stats(10, 3);
                }
        }
    }

    public static Vector2Int[] GetPossibleMovementPoints(EnemyType enemyType, Vector2Int pos)
    {
        //possible movement points list
        List<Vector2Int> possibleMovementPoints = new List<Vector2Int>();

        switch (enemyType)
        {
            case (EnemyType.Pawn):
                {
                    //used for validation
                    //if it finds an unwalkable title interrupts the remaining path tiles
                    bool[] directions = new bool[4] { true, true, true, true };
                    for (int i = 1; i < GetEnemyStats(enemyType).GetMaxMovementPoints() + 1; i++)
                    {
                        if (directions[0])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y)))
                                directions[0] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y));
                        }
                        if (directions[1])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y)))
                                directions[1] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y));
                        }
                        if (directions[2])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x, pos.y + i)))
                                directions[2] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x, pos.y + i));
                        }
                        if (directions[3])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x, pos.y - i)))
                                directions[3] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x, pos.y - i));
                        }
                    }
                    break;
                }
            case (EnemyType.Rook):
                {
                    //used for validation
                    //if it finds an unwalkable title interrupts the remaining path tiles
                    bool[] directions = new bool[4] { true, true, true, true };
                    for (int i = 1; i < GetEnemyStats(enemyType).GetMaxMovementPoints() + 1; i++)
                    {
                        if (directions[0])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y)))
                                directions[0] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y));
                        }
                        if (directions[1])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y)))
                                directions[1] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y));
                        }
                        if (directions[2])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x, pos.y + i)))
                                directions[2] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x, pos.y + i));
                        }
                        if (directions[3])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x, pos.y - i)))
                                directions[3] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x, pos.y - i));
                        }
                    }
                    break;
                }
            case (EnemyType.Bishop):
                {
                    //used for validation
                    //if it finds an unwalkable title interrupts the remaining path tiles
                    bool[] directions = new bool[4] { true, true, true, true };
                    for (int i = 1; i < GetEnemyStats(enemyType).GetMaxMovementPoints() + 1; i++)
                    {
                        if (directions[0])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y - i)))
                                directions[0] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y - i));
                        }
                        if (directions[1])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y + i)))
                                directions[1] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y + i));
                        }
                        if (directions[2])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y + i)))
                                directions[2] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y + i));
                        }
                        if (directions[3])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y - i)))
                                directions[3] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y - i));
                        }
                    }
                    break;
                }
            case (EnemyType.Horse):
                {
                    //check if each space is a walkable
                    if (IsAvailablePosition(new Vector2Int(pos.x - 1, pos.y + 2)))
                        possibleMovementPoints.Add(new Vector2Int(pos.x - 1, pos.y + 2));
                    if (IsAvailablePosition(new Vector2Int(pos.x + 1, pos.y + 2)))
                        possibleMovementPoints.Add(new Vector2Int(pos.x + 1, pos.y + 2));
                    if (IsAvailablePosition(new Vector2Int(pos.x - 1, pos.y - 2)))
                        possibleMovementPoints.Add(new Vector2Int(pos.x - 1, pos.y - 2));
                    if (IsAvailablePosition(new Vector2Int(pos.x + 1, pos.y - 2)))
                        possibleMovementPoints.Add(new Vector2Int(pos.x + 1, pos.y - 2));
                    if (IsAvailablePosition(new Vector2Int(pos.x + 2, pos.y + 1)))
                        possibleMovementPoints.Add(new Vector2Int(pos.x + 2, pos.y + 1));
                    if (IsAvailablePosition(new Vector2Int(pos.x + 2, pos.y - 1)))
                        possibleMovementPoints.Add(new Vector2Int(pos.x + 2, pos.y - 1));
                    if (IsAvailablePosition(new Vector2Int(pos.x - 2, pos.y + 1)))
                        possibleMovementPoints.Add(new Vector2Int(pos.x - 2, pos.y + 1));
                    if (IsAvailablePosition(new Vector2Int(pos.x - 2, pos.y - 1)))
                        possibleMovementPoints.Add(new Vector2Int(pos.x - 2, pos.y - 1));
                    break;
                }
            case (EnemyType.King):
                {
                    //used for validation
                    //if it finds an unwalkable title interrupts the remaining path tiles
                    bool[] directions = new bool[8] { true, true, true, true, true, true, true, true };
                    for (int i = 1; i < GetEnemyStats(enemyType).GetMaxMovementPoints() + 1; i++)
                    {
                        if (directions[0])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y)))
                                directions[0] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y));
                        }
                        if (directions[1])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y)))
                                directions[1] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y));
                        }
                        if (directions[2])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x, pos.y + i)))
                                directions[2] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x, pos.y + i));
                        }
                        if (directions[3])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x, pos.y - i)))
                                directions[3] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x, pos.y - i));
                        }
                        if (directions[4])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y - i)))
                                directions[4] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y - i));
                        }
                        if (directions[5])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y + i)))
                                directions[5] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y + i));
                        }
                        if (directions[6])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y + i)))
                                directions[6] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y + i));
                        }
                        if (directions[7])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y - i)))
                                directions[7] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y - i));
                        }
                    }
                    break;
                }
            case (EnemyType.Queen):
                {
                    //used for validation
                    //if it finds an unwalkable title interrupts the remaining path tiles
                    bool[] directions = new bool[8] { true, true, true, true,true,true,true,true };
                    for (int i = 1; i < GetEnemyStats(enemyType).GetMaxMovementPoints() + 1; i++)
                    {
                        if (directions[0])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y)))
                                directions[0] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y));
                        }
                        if (directions[1])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y)))
                                directions[1] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y));
                        }
                        if (directions[2])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x, pos.y + i)))
                                directions[2] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x, pos.y + i));
                        }
                        if (directions[3])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x, pos.y - i)))
                                directions[3] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x, pos.y - i));
                        }
                        if (directions[4])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y - i)))
                                directions[4] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y - i));
                        }
                        if (directions[5])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y + i)))
                                directions[5] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y + i));
                        }
                        if (directions[6])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y + i)))
                                directions[6] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y + i));
                        }
                        if (directions[7])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y - i)))
                                directions[7] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y - i));
                        }
                    }
                    break;
                }
            default:
                {
                    //used for validation
                    //if it finds an unwalkable title interrupts the remaining path tiles
                    bool[] directions = new bool[4] { true, true, true, true };
                    for (int i = 1; i < GetEnemyStats(enemyType).GetMaxMovementPoints() + 1; i++)
                    {
                        if (directions[0])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x - i, pos.y)))
                                directions[0] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x - i, pos.y));
                        }
                        if (directions[1])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x + i, pos.y)))
                                directions[1] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x + i, pos.y));
                        }
                        if (directions[2])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x, pos.y + i)))
                                directions[2] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x, pos.y + i));
                        }
                        if (directions[3])
                        {
                            if (!IsAvailablePosition(new Vector2Int(pos.x, pos.y - i)))
                                directions[3] = false;
                            else possibleMovementPoints.Add(new Vector2Int(pos.x, pos.y - i));
                        }
                    }
                    break;
                }
        }

        return possibleMovementPoints.ToArray();
    }

    private static bool IsAvailablePosition(Vector2Int pos) 
    {
        bool available = true;

        //check map matrix
        if (MapHandler.GetTileTypeFromMatrix(pos) != MapHandler.TileType.Walkable)
            available = false;

        //check other combatants placement
        for (int z = 0; z < CombatHandler._combatants.Length; z++)
        {
            if (pos == CombatHandler.GetCombatantMatrixPos(z))
            {
                available = false;
                break;
            }
        }

        return available;
    }
}