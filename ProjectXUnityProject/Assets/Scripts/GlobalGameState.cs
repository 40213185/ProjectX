using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalGameState
{
    public enum CombatState { OutOfCombat, Combat }
    public static CombatState combatState { get; private set; }
    public static bool _pause { get; private set; }

    public static void Pause(bool pause) 
    {
        if (pause) Time.timeScale = 0;
        else Time.timeScale = 1.0f;
    }
    public static void SetCombatState(bool inCombat) 
    {
        Time.timeScale = 1.0f;
        if (inCombat)
        {
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().setCombat(true);
            combatState = CombatState.Combat;
        }
        else
        {
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().setCombat(false);
            combatState = CombatState.OutOfCombat;
        }
    }

    public static void UpdateLog(string message)
    {
        if (GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>())
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateLog(message);
    }

    public static void Restart() 
    {
        //save
        GameData.Save();
        //normal game speed
        Time.timeScale = 1.0f;
        //restart
        SceneManager.LoadScene(0);
    }
}
