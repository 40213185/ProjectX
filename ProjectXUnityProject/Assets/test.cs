using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = string.Format("{0} -> {1}\n{2} health",GlobalGameState.combatState.ToString(),
            CombatHandler.GetCurrentTurnCombatant()!=null&&
            CombatHandler.GetCurrentTurnCombatant()!=GameObject.FindGameObjectWithTag("Player")?
            CombatHandler.GetCurrentTurnCombatant().GetComponent<EnemyController>().enemyType.ToString():
            "Player",
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().stats.GetCurrentHealth().ToString());
    }
}
