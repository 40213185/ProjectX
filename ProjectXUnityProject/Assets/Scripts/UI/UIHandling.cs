using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandling : MonoBehaviour
{
    private Stats playerStats;
    private Stats uiPlayerStats;
    private GameObject[] apCollection;
    private GameObject[] mpCOllection;
    

    public Slider hpSlider;

    public GameObject pauseMenu; 

    public GameObject usablesPanel;
    public GameObject apGroup;
    public GameObject mpGroup;
    public GameObject apPoint;
    public GameObject mpPoint;
    public GameObject[] combatObjectCollection;
    public GameObject[] usablesButtons;

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().stats;

        hpSlider.maxValue = playerStats.GetMaxHealth();

        hpSlider.value = playerStats.GetCurrentHealth();

        apCollection = new GameObject[playerStats.GetMaxActionPoints()];
        mpCOllection = new GameObject[playerStats.GetMaxMovementPoints()];

        for (int i = 0; i < playerStats.GetMaxActionPoints(); i++)
        {
            apCollection[i] = Instantiate(apPoint, apGroup.transform);
        }
        for (int i = 0; i < playerStats.GetMaxMovementPoints(); i++)
        {
            apCollection[i] = Instantiate(mpPoint, mpGroup.transform);
        }

        uiPlayerStats = playerStats;
        
    }

    public void PausePressed() 
    {
        GlobalGameState.Pause(true);
        pauseMenu.SetActive(true);
    }

    public void UpdateUI() 
    {
        Debug.Log("Updated");
        if (hpSlider.value != playerStats.GetCurrentHealth())
            hpSlider.value = playerStats.GetCurrentHealth(); 
        
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

    public void BasicAttackPressed() 
    {
        Debug.Log("BASIC ATTACK PRESSED");
    }

    public void WeaponSkillPressed() 
    {
        Debug.Log("WEAPON SKILL USED");
        //InventorySystem.equipmentHeld.getCost();
    }

    public void SkipTurnPressed() 
    {
        Debug.Log("SKIPPING TURN");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerCombat>().EndTurn();
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
