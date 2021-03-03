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
                    return new Stats(12, 4);
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
}
