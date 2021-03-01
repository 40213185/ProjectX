using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatHandler
{
    public static int turnTime { get; private set; }

    //set combatants for turn order and individual turn passing
    public static List<GameObject> _combatants;
    //index pointer for current combatant turn
    private static int combatantPointer;

    private static void ResetCombat() 
    {
        turnTime = 0;
        if (_combatants == null) _combatants = new List<GameObject>();
        else _combatants.Clear();
        combatantPointer = 0;
    }

    public static void Init()
    {
        ResetCombat();
    }
    public static void StartCombat(GameObject combatant)
    {
        //reset combat handler if its the first run
        if (turnTime != 0) ResetCombat();
        //add combatant to the list
        _combatants.Add(combatant);

        //set them to their respective combat start phase state
        if (combatant.tag == "Player") combatant.GetComponent<PlayerController>().CombatStartPhase();
        else if (combatant.tag == "Enemy") combatant.GetComponent<EnemyController>().CombatStart();
    }

    public static void AddTurnTime()
    {
        turnTime++;
    }
    public static void ResetTurnTime()
    {
        turnTime = 0;
    }

    public static void NextCombatantTurn() 
    {
        //end turn
        if (_combatants[combatantPointer].tag == "Player") _combatants[combatantPointer].GetComponent<PlayerController>().EndTurn();
        else if (_combatants[combatantPointer].tag == "Enemy") _combatants[combatantPointer].GetComponent<EnemyController>().EndTurn();

        //increment counter
        if (combatantPointer >= _combatants.Count)
        {
            AddTurnTime();
            combatantPointer = 0;
        }
        else combatantPointer++;

        //set turn for next combatant
        if (_combatants[combatantPointer].tag == "Player") _combatants[combatantPointer].GetComponent<PlayerController>().MyTurn();
        else if (_combatants[combatantPointer].tag == "Enemy") _combatants[combatantPointer].GetComponent<EnemyController>().MyTurn();

    }
}