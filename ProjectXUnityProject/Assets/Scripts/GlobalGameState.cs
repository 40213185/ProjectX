using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalGameState
{
    public enum CombatState { OutOfCombat, Combat }
    public static CombatState combatState { get; private set; }
    public static bool _pause { get; private set; }

    public static void Pause(bool pause) 
    {
        _pause = pause;
        if (_pause) Time.timeScale = 0;
        else Time.timeScale = 1.0f;
    }
    public static void SetCombatState(bool inCombat) 
    {
        if (inCombat) combatState = CombatState.Combat;
        else combatState = CombatState.OutOfCombat;
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().setCombat(inCombat);
    }
}
