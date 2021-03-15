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

    public void BasicAttackPressed(int index) 
    {
        Debug.Log("BASIC ATTACK PRESSED");
    }

    public void WeaponSkillPressed(int index) 
    {
        Debug.Log("WEAPON SKILL USED");
    }

    public void UsablesToggle() 
    {
        Animator anim = usablesPanel.GetComponent<Animator>();
        bool isOpen = anim.GetBool("?Open");
        anim.SetBool("?Open", !isOpen);
    }

    public void UsablePressed(int index) 
    {
        Debug.Log("USABLE " + index + " PRESSED");
    }
}
