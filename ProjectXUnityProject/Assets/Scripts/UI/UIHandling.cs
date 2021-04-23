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

    //ItemTooltips
    public GameObject itemToolTip;
    public GameObject toolTipBackground;
    public Image itemToolTipImage;
    public Text itemToolTipText;
    public Sprite[] weaponIcons;
    private bool showingTooltip = false;

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
        playerStats = GameData.stats;
        ap = playerStats.MaximumActionPointsAllowed();
        mp = playerStats.MaximumMovementPointsAllowed();
        hpSlider.maxValue = playerStats.GetMaxHealth();
        hpText.text = string.Format("{0}/{1}",playerStats.GetCurrentHealth(),playerStats.GetMaxHealth());

        hpSlider.value = playerStats.GetCurrentHealth();

        PopulateAp(ap);
        PopulateMp(mp);

        //combat controller
        playerCombatController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerCombat>();

        UpdateUI();
    }
    private void PopulateAp(int size)
    {
        apCollection = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            apCollection[i] = Instantiate(apPoint, apGroup.transform);
        }
    }
    private void PopulateMp(int size)
    {
        mpCollection = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            mpCollection[i] = Instantiate(mpPoint, mpGroup.transform);
        }
    }

    public void PausePressed() 
    {
        GlobalGameState.Pause(true);
        pauseMenu.SetActive(true);
    }

    public void UpdateUI() 
    {
        if (playerStats != null)
        {
            if (hpSlider.value != playerStats.GetCurrentHealth())
            {
                //update slider
                hpSlider.value = playerStats.GetCurrentHealth();
                //update text
                hpText.text = string.Format("{0}/{1}", playerStats.GetCurrentHealth(), playerStats.GetMaxHealth());
            }

            //ap handling
            if (ap != playerStats.GetCurrentActionPoints())
            {
                ap = playerStats.GetCurrentActionPoints();
                foreach (GameObject e in apCollection)
                {
                    e.SetActive(false);
                }
                for (int i = 0; i < playerStats.GetCurrentActionPoints(); i++)
                {
                    apCollection[i].SetActive(true);
                }
            }
            //mp handling
            if (mp != playerStats.GetCurrentMovementPoints())
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
    }

    public void UpdateLog(string message)
    {
        GameObject text=Instantiate(logTextPrefab, logContent.transform);
        text.GetComponent<Text>().text = message;
        StartCoroutine("ChatScrollQuickFix");
    }

    private IEnumerator ChatScrollQuickFix() 
    {
        yield return new WaitForSecondsRealtime(0.1f);
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

    public void ShowToolTipWeapon(Weapon.EquipmentType type, string name, int rarity, Vector2Int potency, string description)
    {
        string rarityText = null;

        Cursor.visible = false;

        switch (rarity)
        {
            case 1:
                rarityText = "Common";
                break;
            case 2:
                rarityText = "Uncommon";
                break;
            case 3:
                rarityText = "Rare";
                break;
            case 4:
                rarityText = "Unique";
                break;
        }


        itemToolTip.SetActive(true);
        showingTooltip = true;
        itemToolTipImage.enabled = true;

        itemToolTipText.text = name + "\n\n" + rarityText + "\n\nDamage: " + potency + "\n\n" + description;

        Debug.Log(name + rarityText + "Damage: " + potency + description);

        //Resize tooltip according to the size of the text
        Vector2 backgroundSize = new Vector2(0, 0);
        sizeTooltip();


        itemToolTip.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }

    public float getTooltipSize() 
    {
        return itemToolTipText.preferredHeight + 4;
    }

    public void ShowToolTipUsable(Usable.UsableType type, string name, Vector2Int potency, string description)
    {
        
       
        itemToolTip.SetActive(true);
        itemToolTipImage.enabled = true;
        showingTooltip = true;
        itemToolTipText.text = name + "\n\nPotency: " + potency + "\n\n" + description;


        //Resize tooltip according to the size of the text
        sizeTooltip();

        itemToolTip.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }

    public void toolTipMessage(string message)
    {
        itemToolTip.SetActive(true);
        itemToolTipImage.enabled = false;
        showingTooltip = true;

        itemToolTipText.text = message;

        sizeTooltip();

        itemToolTip.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }

    private void sizeTooltip() 
    {
        Vector2 backgroundSize = new Vector2(0, 0);
        if (itemToolTipText.preferredHeight > 150)
        {
            //Have to do this as first value for some reason is always in the thousands, works after that point, temp fix but mainly for presentation so it doesnt look like shit
            backgroundSize = new Vector2(itemToolTipText.preferredWidth, 150 + 4f * 4f);
            toolTipBackground.GetComponent<RectTransform>().sizeDelta = backgroundSize;
            itemToolTipText.GetComponent<RectTransform>().sizeDelta = backgroundSize;
            Debug.Log(itemToolTipText.preferredHeight);
        }
        else
        {
            backgroundSize = new Vector2(itemToolTipText.preferredWidth, itemToolTipText.preferredHeight + 4f * 4f);
            toolTipBackground.GetComponent<RectTransform>().sizeDelta = backgroundSize;
            itemToolTipText.GetComponent<RectTransform>().sizeDelta = backgroundSize;
        }
    }

    public void HideToolTip()
    {
        Cursor.visible = true;
        itemToolTip.SetActive(false);
        showingTooltip = false;
    }

    public void BasicAttackPressed() 
    {
        //check if not moving
        if (playerCombatController.combatControllerState == PlayerControllerCombat.CombatControllerState.Wait)
        {
            //check if can attack
            if (playerStats.GetCurrentActionPoints() >= 1)
            {
                playerCombatController.SelectAction(InventorySystem.equipmentHeld,false);
            }
            //check if cant use anymore
            if (playerStats.GetCurrentActionPoints() < 1)
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
        }
        if(playerStats.GetCurrentActionPoints()>0)
        { 
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
        GameObject[] objs = new GameObject[1] { playerCombatController.gameObject };
        InventorySystem.usablesHeld[index].Use(objs);
        usablesButtons[index].SetActive(false);
        Debug.Log("USABLE " + index + " PRESSED");
        InventorySystem.usablesHeld[index] = null;
    }

    public void bagToolTip(int index) 
    {
        Usable.UsableType type = InventorySystem.usablesHeld[index].GetUsableType();
        string name = InventorySystem.usablesHeld[index].GetName();
        Vector2Int potency = InventorySystem.usablesHeld[index].GetPotency();
        string description = InventorySystem.usablesHeld[index].GetDescription();

        ShowToolTipUsable(type, name, potency, description);
    }

    public void currentWepTooltip() 
    {
        Weapon.EquipmentType type = InventorySystem.equipmentHeld.GetEquipmentType();
        string name = InventorySystem.equipmentHeld.GetName();
        int rarity = InventorySystem.equipmentHeld.getRarity();
        Vector2Int potency = InventorySystem.equipmentHeld.GetPotency();
        string description = InventorySystem.equipmentHeld.GetDescription();

        ShowToolTipWeapon(type, name, rarity, potency, description);
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
