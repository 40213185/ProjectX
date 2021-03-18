using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandling : MonoBehaviour
{
    private Stats playerStats;
    private Stats uiPlayerStats;
    private GameObject[] apCollection;
    

    public Slider hpSlider;
    public Slider manaSlider;

    public GameObject usablesPanel;
    public GameObject apGroup;
    public GameObject apPoint;
    public GameObject[] combatObjectCollection;
    public GameObject[] usablesButtons;

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetStats();

        hpSlider.maxValue = playerStats.GetMaxHealth();

        hpSlider.value = playerStats.GetCurrentHealth();

        apCollection = new GameObject[playerStats.GetMaxActionPoints()];
        for (int i = 0; i < playerStats.GetMaxActionPoints(); i++)
        {
            apCollection[i] = Instantiate(apPoint, apGroup.transform);
            Debug.Log(i);
        }

        uiPlayerStats = playerStats;
        
    }

    public void UpdateUI() 
    {

        if (uiPlayerStats.GetCurrentHealth() != playerStats.GetCurrentHealth()) hpSlider.value = playerStats.GetCurrentHealth(); 
        
        if(uiPlayerStats.GetCurrentActionPoints() != playerStats.GetCurrentActionPoints()) 
        {
            foreach(GameObject e in apCollection) 
            {
                e.SetActive(false);
            }
            for(int i = 0; i < playerStats.GetCurrentActionPoints(); i++) 
            {
                apCollection[i].SetActive(true);
            }
        }
    }

    public void setCombat(bool inCombat) 
    {
        if (inCombat == false)
        {
            foreach (GameObject e in combatObjectCollection)
            {
                e.SetActive(false);
            }
        }
        else 
        {
            foreach (GameObject e in combatObjectCollection)
            {
                e.SetActive(true);
            }
        }
    }

    public void BasicAttackPressed(int index) 
    {
        Debug.Log("BASIC ATTACK PRESSED");
    }

    public void WeaponSkillPressed(int index) 
    {
        Debug.Log("WEAPON SKILL USED");
    }

    public void SkipTurnPressed() 
    {
        Debug.Log("SKIPPING TURN");
    }

    public void UsablesToggle() 
    {
        Animator anim = usablesPanel.GetComponent<Animator>();
        bool isOpen = anim.GetBool("?Open");
        anim.SetBool("?Open", !isOpen);
        int count = 0;

        foreach(Usable e in InventorySystem.usablesHeld) 
        {
            if (e != null) usablesButtons[count].SetActive(true) ; else usablesButtons[count].SetActive(false);
            count++;
        }
    }

    public void UsablePressed(int index) 
    {
        usablesButtons[index].SetActive(false);
        Debug.Log("USABLE " + index + " PRESSED");
        InventorySystem.usablesHeld[index] = null;
    }

}
