using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static DataContainer dataContainer;
    public static int CurrentFloor { get; private set; }

    public static int currency { get; private set; }
    public static Stats stats { get; private set; }

    private static void Init()
    {
        if (dataContainer == null) dataContainer = new DataContainer();
        if (stats == null) stats = new Stats();
    }

    public static void Save()
    {
        Init();
        dataContainer.SetData();
        dataContainer.SaveData();
    }

    public static bool Load() 
    {
        Init();
        if (dataContainer.LoadData())
        {
            //set the data container data back to game data
            currency = dataContainer.currency;
            stats = dataContainer.stats;
            return true;
        }
        return false;
    }

    public static void SetFloor(int floor) 
    {
        CurrentFloor = floor;
    }

    public static int ModifyCurrencyBy(int amount)
    {
        currency = Mathf.Clamp(currency + amount, 0, 999999);
        return amount;
    }
}
