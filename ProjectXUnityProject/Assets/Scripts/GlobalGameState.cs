using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalGameState
{
    public enum CombatState { Combat,OutOfCombat }
    public static CombatState combatState { get; private set; }
    public static bool _pause { get; private set; }
    public static int turnTime { get; private set; }

    public static void Pause(bool pause) 
    {
        _pause = pause;
    }
    public static void AddTurnTime() 
    {
        turnTime++;
    }
    public static void ResetTurnTime() 
    {
        turnTime = 0;
    }
    public static void SetCombatState(bool inCombat) 
    {
        if (inCombat) combatState = CombatState.Combat;
        else combatState = CombatState.OutOfCombat;
    }
}
