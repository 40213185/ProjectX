using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandling : MonoBehaviour
{
    private Stats playerStats;
    private GameObject[] apCollection;
    private GameObject[] mpCollection;
    

    public Slider hpSlider;

    public GameObject pauseMenu; 

    public GameObject usablesPanel;
    public GameObject apGroup;
    public GameObject mpGroup;
    public GameObject apPoint;
    private int ap;
    public GameObject mpPoint;
    private int mp;
    public GameObject[] combatObjectCollection;
    public GameObject[] usablesButtons;

    //combat controller
    private PlayerControllerCombat playerCombatController;

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().stats;
        ap = playerStats.GetCurrentActionPoints();
        mp = playerStats.GetCurrentMovementPoints();
        hpSlider.maxValue = playerStats.GetMaxHealth();

        hpSlider.value = playerStats.GetCurrentHealth();

        apCollection = new GameObject[playerStats.GetMaxActionPoints()];
        mpCollection = new GameObject[playerStats.GetMaxMovementPoints()];

        for (int i = 0; i < playerStats.GetMaxActionPoints(); i++)
        {
            apCollection[i] = Instantiate(apPoint, apGroup.transform);
        }
        for (int i = 0; i < playerStats.GetMaxMovementPoints(); i++)
        {
            mpCollection[i] = Instantiate(mpPoint, mpGroup.transform);
        }

        //combat controller
        playerCombatController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerCombat>();
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
        
        //ap handling
        if( ap!= playerStats.GetCurrentActionPoints()) 
        {
            ap = playerStats.GetCurrentActionPoints();
            foreach (GameObject e in apCollection) 
            {
                e.SetActive(false);
            }
            for(int i = 0; i < playerStats.GetCurrentActionPoints(); i++) 
            {
                apCollection[i].SetActive(true);
            }
        }
        //mp handling
        if ( mp!= playerStats.GetCurrentMovementPoints())
        {
            mp = playerStats.GetCurrentMovementPoints();
            foreach (GameObject e in mpCollection)
            {
                e.SetActive(false);
            }
            for (int i = 0; i < playerStats.GetCurrentMovementPoints(); i++)
            {
                mpCollection[i].SetActive(true);
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
        if (playerStats.GetCurrentActionPoints() >= InventorySystem.equipmentHeld.getCost())
        {
            playerCombatController.SelectAction(InventorySystem.equipmentHeld);
        }
    }

    public void SkipTurnPressed() 
    {
        Debug.Log("SKIPPING TURN");
        playerCombatController.EndTurn();
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
