using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradesUIHandler : MonoBehaviour
{
    public GameObject upgradesContainer;
    public Text currencyText;
    public Button upgradeHp, upgradeAp, upgradeMp;

    public int hpCost, apCost, mpCost;
    public int hpAmount, apAmount, mpAmount;

    public enum UpgradeType
    {
        HP,
        AP,
        MP
    }

    // Start is called before the first frame update
    void Start()
    {
        GameData.Load();
        UpdateUI();
    }

    private void UpdateUI()
    {
        currencyText.text = GameData.currency.ToString();

        if (GameData.currency < hpCost) upgradeHp.enabled = false;
        else upgradeHp.enabled = true;
        if (GameData.currency < apCost || GameData.stats.GetCurrentActionPoints() >= GameData.stats.MaximumActionPointsAllowed()) upgradeAp.enabled = false;
        else upgradeAp.enabled = true;
        if (GameData.currency < mpCost || GameData.stats.GetCurrentMovementPoints() >= GameData.stats.MaximumMovementPointsAllowed()) upgradeMp.enabled = false;
        else upgradeMp.enabled = true;
    }

    public void Upgrade(int upgradeType)
    {
        switch (upgradeType)
        {
            case 0:
                {
                    GameData.stats.ModifyMaxHpBy(hpAmount);
                    GameData.ModifyCurrencyBy(-hpCost);
                    break;
                }
            case 1:
                {
                    GameData.stats.ModifyMaxApBy(apAmount);
                    GameData.ModifyCurrencyBy(-apCost);
                    break;
                }
            case 2:
                {
                    GameData.stats.ModifyMaxMpBy(mpAmount);
                    GameData.ModifyCurrencyBy(-mpCost);
                    break;
                }
        }
        SoundbankHandler.SoundEvent(SoundbankHandler.Sounds.Play_Coins, gameObject);
        GameData.Save();
        UpdateUI();
    }

    public void BeginGame()
    {
        Debug.Log("Begin");
        SoundbankHandler.SoundEvent(SoundbankHandler.Sounds.Play_UI_Menu_1, gameObject);
        SoundbankHandler.SoundEvent(SoundbankHandler.Sounds.Stop_Main_Menu_Blend, gameObject);
        SceneManager.LoadScene("Dungeon");
    }

}
