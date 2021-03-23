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

    public Stats(int maxhealth,int maxmovementPoints,int maxactionPoints) 
    {
        maxHealth = maxhealth;
        currentHealth = maxHealth;
        maxMovementPoints = maxmovementPoints;
        currentMovementPoints = maxMovementPoints;
        maxActionPoints = maxactionPoints;
        currentActionPoints = maxActionPoints;
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

    public int GetCurrentMovementPoints() 
    {
        return currentMovementPoints;
    }
    public int GetMaxMovementPoints() 
    {
        return maxMovementPoints;
    }
}
