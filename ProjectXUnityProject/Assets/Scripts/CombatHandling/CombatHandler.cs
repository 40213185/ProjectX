using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatHandler
{
    public static int turnTime { get; private set; }

    //set combatants for turn order and individual turn passing
    public static GameObject[] _combatants;
    //index pointer for current combatant turn
    private static int combatantPointer;

    private static void ResetCombat(GameObject[] combatants) 
    {
        //set list of combatants/battle participants
        _combatants = combatants; 
        
        Debug.Log(_combatants.Length.ToString());
        for (int i = 0; i < _combatants.Length; i++)
        {
            if (_combatants[i] == null) Debug.Log("null");else Debug.Log(_combatants[i].tag .ToString());
        }
        //reset turn time
        turnTime = 0;
        //set pointer to the first combatant index
        combatantPointer = 0;
    }

    public static void StartCombat(GameObject[] combatants)
    {
        //set global state
        GlobalGameState.SetCombatState(true);

        //if its the fist call of combat handling
        if (_combatants==null) CombatHandler.ResetCombat(combatants);
        for (int i = 0; i < _combatants.Length; i++)
        {
            //set them to their respective combat start phase state
            if (_combatants[i].transform.tag == "Player") _combatants[i].GetComponent<PlayerController>().CombatStartPhase();
            else if (_combatants[i].transform.tag == "Enemy") _combatants[i].GetComponent<EnemyController>().CombatStart();
        }
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
        if (_combatants[combatantPointer].tag == "Player") _combatants[combatantPointer].GetComponent<PlayerControllerCombat>().EndTurn();
        else if (_combatants[combatantPointer].tag == "Enemy") _combatants[combatantPointer].GetComponent<EnemyController>().EndTurn();

        //increment counter
        if (combatantPointer >= _combatants.Length-1)
        {
            AddTurnTime();
            combatantPointer = 0;
        }
        else combatantPointer++;

        //set turn for next combatant
        if (_combatants[combatantPointer].tag == "Player") _combatants[combatantPointer].GetComponent<PlayerControllerCombat>().MyTurn();
        else if (_combatants[combatantPointer].tag == "Enemy") _combatants[combatantPointer].GetComponent<EnemyController>().MyTurn();

    }

    public static void EndCombat() 
    {
        //clear combatants list
        _combatants=null;
    }
}