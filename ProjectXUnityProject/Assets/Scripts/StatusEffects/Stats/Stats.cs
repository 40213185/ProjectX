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

    public Stats(int maxhealth,int maxmovementPoints,int maxactionPoints,int initiative) 
    {
        maxHealth = maxhealth;
        currentHealth = maxHealth;
        maxMovementPoints = maxmovementPoints;
        currentMovementPoints = maxMovementPoints;
        maxActionPoints = maxactionPoints;
        currentActionPoints = maxActionPoints;
        baseInitiative = initiative;
    }

    public int GetMaxHealth() 
    {
        return maxHealth;
    }
    public int GetCurrentHealth() 
    {
        return currentHealth;
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
        amount = maxActionPoints - amount;
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
        int amount=currentMovementPoints;
        currentMovementPoints = maxMovementPoints;
        amount = maxMovementPoints - amount;
        //ui
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().UpdateUI();
        return amount;
    }

    public int RollInitiative()
    {
        int temp = UnityEngine.Random.Range(1, 7);
        temp += baseInitiative;
        return temp;
    }
}
