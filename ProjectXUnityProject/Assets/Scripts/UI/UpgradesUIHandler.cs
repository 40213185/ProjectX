using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        if (GameData.currency > 0)
        {
            upgradesContainer.SetActive(true);
            UpdateUI();
        }
        else upgradesContainer.SetActive(false);
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

        GameData.Save();
        UpdateUI();
    }
}
