using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    private int maxHealth;
    private int currentHealth;

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
}
