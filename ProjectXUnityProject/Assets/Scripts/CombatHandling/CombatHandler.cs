using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatHandler
{
    public static int turnTime { get; private set; }

    //set combatants for turn order and individual turn passing
    public static GameObject[] _combatants { get; private set; }
    //index pointer for current combatant turn
    private static int combatantPointer;

    private static void ResetCombat(GameObject[] combatants)
    {
        //set list of combatants/battle participants
        _combatants = combatants;

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
        ResetCombat(combatants);
        for (int i = 0; i < _combatants.Length; i++)
        {
            //set them to their respective combat start phase state
            if (_combatants[i].transform.tag == "Player") _combatants[i].GetComponent<PlayerController>().CombatStartPhase();
            else if (_combatants[i].transform.tag == "Enemy") _combatants[i].GetComponent<EnemyController>().CombatStart();
        }

        //order combatants according to initiative roles
        int[] rolls = new int[_combatants.Length];
        //roll
        for (int i = 0; i < _combatants.Length; i++) 
        {
            if (_combatants[i].GetComponent<PlayerController>())
                rolls[i] = _combatants[i].GetComponent<PlayerController>().stats.RollInitiative();
            else if (_combatants[i].GetComponent<EnemyController>())
            {
                if (_combatants[i].GetComponent<EnemyController>().stats == null)
                {
                    _combatants[i].GetComponent<EnemyController>().SetStats();
                }
                rolls[i] = _combatants[i].GetComponent<EnemyController>().stats.RollInitiative();
            }
        }

        //sort
        bool sorted = false;
        GameObject tempSortCombatant;
        int tempSortRoll;
        int tempSortIndex = 0;
        int startIndex = 0;
        while (!sorted)
        {
            //reset roll for check
            tempSortRoll = 0;
            //go through combatants
            for (int i =startIndex; i < _combatants.Length; i++)
            {
                //find highest roll and store index
                if (rolls[i] > tempSortRoll)
                {
                    tempSortRoll = rolls[i];
                    tempSortIndex = i;
                }
            }
            //swap combatants
            tempSortCombatant = _combatants[startIndex];
            _combatants[startIndex] = _combatants[tempSortIndex];
            _combatants[tempSortIndex] = tempSortCombatant;
            //swap rolls
            tempSortRoll = rolls[startIndex];
            rolls[startIndex] = rolls[tempSortIndex];
            rolls[tempSortIndex] = tempSortRoll;
            //check
            sorted = true;
            for (int i = 0; i < _combatants.Length - 1; i++)
            {
                if (rolls[i] < rolls[i + 1])
                {
                    sorted = false;
                    break;
                }
            }
            //increase for next highest
            startIndex++;
            if (startIndex >= _combatants.Length) startIndex=0;
        }

        //set the first combatant to its turn start
        if (_combatants[0].transform.tag == "Player") _combatants[0].GetComponent<PlayerControllerCombat>().MyTurn();
        else if (_combatants[0].transform.tag == "Enemy") _combatants[0].GetComponent<EnemyController>().MyTurn();
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
        //increment counter
        if (combatantPointer >= _combatants.Length - 1)
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
        //in case of speed up
        if (Time.timeScale > 1)
            Time.timeScale = 1;
        //clear combatants list
        _combatants = null;
    }

    public static Vector2Int GetCombatantMatrixPos(int index)
    {
        if (index < _combatants.Length)
        {
            Vector2Int matrixPos = new Vector2Int(Mathf.FloorToInt(_combatants[index].transform.position.x),
                Mathf.FloorToInt(_combatants[index].transform.position.z));

            return matrixPos;
        }
        else return new Vector2Int(0, 0);
    }

    public static GameObject GetCurrentTurnCombatant()
    {
        if (_combatants != null)
            return _combatants[combatantPointer];
        else return null;
    }
    public static GameObject getNextTurnCombatant()
    {
        if (_combatants != null)
        {
            int nextPointer = combatantPointer + 1;
            if (nextPointer >= _combatants.Length) nextPointer = 0;
            return _combatants[nextPointer];
        }
        else return null;
    }
}