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
    public Text hpText;

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

    //disabling and enabling objects
    public GameObject skipTurnButton;
    public GameObject basicAttack;
    public GameObject weaponSkill;
    public GameObject itemsPanel;

    //log
    public GameObject logContent;
    public GameObject logTextPrefab;
    public GameObject logScrollBar;

    public GameObject followButton;

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().stats;
        ap = playerStats.GetCurrentActionPoints();
        mp = playerStats.GetCurrentMovementPoints();
        hpSlider.maxValue = playerStats.GetMaxHealth();
        hpText.text = string.Format("{0}/{1}",playerStats.GetCurrentHealth(),playerStats.GetMaxHealth());

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
        if (hpSlider.value != playerStats.GetCurrentHealth())
        {
            //update slider
            hpSlider.value = playerStats.GetCurrentHealth();
            //update text
            hpText.text = string.Format("{0}/{1}", playerStats.GetCurrentHealth(), playerStats.GetMaxHealth());
        }
        
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

    public void UpdateLog(string message)
    {
        GameObject text=Instantiate(logTextPrefab, logContent.transform);
        text.GetComponent<Text>().text = message;
        StartCoroutine("ChatScrollQuickFix");
    }

    private IEnumerator ChatScrollQuickFix() 
    {
        yield return new WaitForSeconds(0.25f);
        logScrollBar.GetComponent<Scrollbar>().value = 0;
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
        //check if not moving
        if (playerCombatController.combatControllerState == PlayerControllerCombat.CombatControllerState.Wait)
        {
            //check if can attack
            if (playerStats.GetCurrentActionPoints() >= InventorySystem.equipmentHeld.getCost())
            {
                playerCombatController.SelectAction(InventorySystem.equipmentHeld,false);
            }
            //check if cant use anymore
            if (playerStats.GetCurrentActionPoints() < InventorySystem.equipmentHeld.getCost())
            {
                basicAttack.GetComponent<Button>().interactable = false;
                weaponSkill.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void WeaponSkillPressed()
    {
        //check if not moving
        if (playerCombatController.combatControllerState == PlayerControllerCombat.CombatControllerState.Wait)
        {
            //check if can attack
            if (playerStats.GetCurrentActionPoints() >= InventorySystem.equipmentHeld.getCost())
            {
                playerCombatController.SelectAction(InventorySystem.equipmentHeld,true);
            }
            //check if cant use anymore
            if (playerStats.GetCurrentActionPoints() < InventorySystem.equipmentHeld.getCost())
            {
                basicAttack.GetComponent<Button>().interactable = false;
                weaponSkill.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void SkipTurnPressed()
    {
            if (playerCombatController.combatControllerState == PlayerControllerCombat.CombatControllerState.Wait||
            playerCombatController.combatControllerState==PlayerControllerCombat.CombatControllerState.SelectAction)
        {
            if(playerCombatController.isMyTurn()) playerCombatController.EndTurn();
            //disable button
            if (!playerCombatController.isMyTurn())
            {
                skipTurnButton.GetComponent<Button>().interactable = false;
                //disable other buttons
                weaponSkill.GetComponent<Button>().interactable = false;
                basicAttack.GetComponent<Button>().interactable = false;
                itemsPanel.SetActive(false);
            }
        }
    }

    public void ReEnableButtonsForTurnStart()
    {
        //enable skip turn button
        skipTurnButton.GetComponent<Button>().interactable = true;
        //enable other buttons
        if (playerStats.GetCurrentActionPoints() >= InventorySystem.equipmentHeld.getCost())
        {
            weaponSkill.GetComponent<Button>().interactable = true;
            basicAttack.GetComponent<Button>().interactable = true;
        }
        itemsPanel.SetActive(true);
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

    public void FollowPlayer()
    {
        GameObject.FindGameObjectWithTag("FreeCam").GetComponent<CamMovement>().newTarget(GameObject.FindGameObjectWithTag("Player"));
        FollowButtonToggle(false);
    }

    public void FollowButtonToggle(bool active)
    {
        followButton.SetActive(active);
    }
}
