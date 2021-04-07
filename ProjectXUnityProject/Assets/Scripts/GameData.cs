using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static DataContainer dataContainer;
    public static int CurrentFloor { get; private set; }

    public static int currency { get; private set; }

    public static void Save() 
    {
        if (dataContainer == null)
        {
            dataContainer = new DataContainer();
        }
        dataContainer.SetData(dataContainer);
        dataContainer.SaveData();
    }

    public static void Load() 
    {
        if (dataContainer == null) dataContainer = new DataContainer();
        dataContainer.LoadData();
        //set the data container data back to game data
        currency = dataContainer.currency;
    }

    public static void SetFloor(int floor) 
    {
        CurrentFloor = floor;
    }

    public static void ModifyCurrencyBy(int amount)
    {
        currency = Mathf.Clamp(currency + amount, 0, 999999);
    }
}
