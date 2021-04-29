using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFight()
    {
        //add combatants to list
        GameObject[] combatants = new GameObject[GameObject.FindGameObjectsWithTag("Enemy").Length+1];
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
        {
            combatants[i] = GameObject.FindGameObjectsWithTag("Enemy")[i];
        }
        //add player to combatants list
        combatants[combatants.Length - 1] = GameObject.FindGameObjectWithTag("Player");

        //enemies spawned? start a fight
        CombatHandler.StartCombat(combatants);

        //disable component after use
        enabled = false;
    }
}
