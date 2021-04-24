using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    private int maxHealth;
    private int currentHealth;
    private int maxMovementPoints;
    private int currentMovementPoints;
    private int maxActionPoints;
    private int currentActionPoints;
    private int baseInitiative;

    private int maxActionPointsAllowed;
    private int maxMovementPointsAllowed;

    public Stats()
    {
        maxHealth = 10;
        currentHealth = maxHealth;
        maxMovementPoints = 3;
        currentMovementPoints = maxMovementPoints;
        maxActionPoints = 6;
        currentActionPoints = maxActionPoints;
        baseInitiative = 3;

        maxActionPointsAllowed = 15;
        maxMovementPointsAllowed = 15;
    }

    public Stats(int maxhealth,int maxmovementPoints,int maxactionPoints,int initiative) 
    {
        maxHealth = maxhealth;
        currentHealth = maxHealth;
        maxMovementPoints = maxmovementPoints;
        currentMovementPoints = maxMovementPoints;
        maxActionPoints = maxactionPoints;
        currentActionPoints = maxActionPoints;
        baseInitiative = initiative;

        maxActionPointsAllowed = 13;
        maxMovementPointsAllowed = 13;
    }

    public void SetStats(Stats stats)
    {
        maxHealth = stats.GetMaxHealth();
        currentHealth = stats.GetCurrentHealth();
        maxMovementPoints = stats.GetMaxMovementPoints();
        currentMovementPoints = stats.GetCurrentMovementPoints();
        maxActionPoints = stats.GetMaxActionPoints();
        currentActionPoints = stats.GetCurrentActionPoints();
        baseInitiative = stats.GetBaseInitiative();
    }

    public int GetMaxHealth() 
    {
        return maxHealth;
    }
    public int GetCurrentHealth() 
    {
        return currentHealth;
    }
    public int RefillHealth()
    {
        int amount = currentHealth;
        currentHealth = maxHealth;
        amount = currentHealth - amount;
        //ui
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateUI();
        return amount;
    }
    public int ModifyHealthBy(int amount) 
    {
        //store current health
        int beforeHealth = currentHealth;
        //make changes
        currentHealth = Mathf.Clamp(currentHealth+amount,0,maxHealth);
        //update ui
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateUI();
        //return the difference between previous current health and new one
        return beforeHealth - currentHealth;
    }

    public int GetCurrentActionPoints()
    {
        return currentActionPoints;
    }

    public int GetMaxActionPoints()
    {
        return maxActionPoints;
    }

    public int ModifyActionPointsBy(int amount)
    {
        //store current ap
        int beforeAP = currentActionPoints;
        //make changes
        currentActionPoints = Mathf.Clamp(currentActionPoints + amount, 0, maxActionPoints);
        //update ui
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateUI();
        //return the difference between previous current ap and new one
        return beforeAP - currentActionPoints;
    }

    public int RefillActionPoints()
    {
        int amount = currentActionPoints;
        currentActionPoints = maxActionPoints;
        amount = currentActionPoints - amount;
        //ui
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateUI();
        return amount;
    }

    public int GetCurrentMovementPoints() 
    {
        return currentMovementPoints;
    }
    public int GetMaxMovementPoints() 
    {
        return maxMovementPoints;
    }

    public int ModifyMovementPointsBy(int amount) 
    {
        //store current mp
        int beforeMP = currentHealth;
        //make changes
        currentMovementPoints = Mathf.Clamp(currentMovementPoints + amount, 0, maxMovementPoints);
        //update ui
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateUI();
        //return the difference between previous current mp and new one
        return beforeMP - currentMovementPoints;
    }

    public int RefillMovementPoints()
    {
        int amount = currentMovementPoints;
        currentMovementPoints = maxMovementPoints;
        amount = maxMovementPoints - amount;
        //ui
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateUI();
        return amount;
    }
    public int GetBaseInitiative() 
    {
        return baseInitiative;
    }
    public int RollInitiative()
    {
        int temp = UnityEngine.Random.Range(1, 7);
        temp += baseInitiative;
        return temp;
    }
    public int MaximumActionPointsAllowed()
    {
        return maxActionPointsAllowed;
    }

    public int MaximumMovementPointsAllowed() 
    {
        return maxMovementPointsAllowed;
    }

    public int ModifyMaxHpBy(int amount)
    {
        int dif = maxHealth;
        maxHealth = Mathf.Clamp(maxHealth + amount, 0, int.MaxValue);
        dif = maxHealth - dif;
        return dif;
    }
    public int ModifyMaxApBy(int amount)
    {
        int dif = maxActionPoints;
        maxActionPoints = Mathf.Clamp(maxActionPoints + amount, 0, maxActionPointsAllowed);
        dif = maxActionPoints - dif;
        return dif;
    }
    public int ModifyMaxMpBy(int amount)
    {
        int dif = maxMovementPoints;
        maxMovementPoints = Mathf.Clamp(maxMovementPoints + amount, 0, maxMovementPointsAllowed);
        dif = maxMovementPoints - dif;
        return dif;
    }
}
