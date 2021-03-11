﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static DataContainer dataContainer;
    public static int CurrentFloor { get; private set; }

    public static void Save() 
    {
        dataContainer.SetData(dataContainer);
        dataContainer.SaveData();
    }

    public static void Load() 
    {
        dataContainer.LoadData();
        //set the data container data back to game data
        ///
        ///write code here
        ///
    }

    public static void SetFloor(int floor) 
    {
        CurrentFloor = floor;
    }
}
