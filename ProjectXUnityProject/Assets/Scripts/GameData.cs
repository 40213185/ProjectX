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
        dataContainer = new DataContainer();
        currency = 0;
        stats = new Stats();
    }

    public static void Save()
    {
        if (dataContainer == null) Init();
        dataContainer.SetData();
        dataContainer.SaveData();
    }

    public static bool Load() 
    {
        if (dataContainer == null) Init();
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
