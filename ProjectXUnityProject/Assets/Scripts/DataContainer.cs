using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DataContainer
{
    //container data
    public int currency;

    public int maxHealth;
    public int maxMovementPoints;
    public int maxActionPoints;
    public int baseInitiative;

    public Stats stats;

    //container methods
    string path = Application.persistentDataPath + "/GameData.dat";

    public DataContainer() 
    {
        currency = 0;
        maxHealth = 0;
        maxMovementPoints = 0;
        maxActionPoints = 0;
        baseInitiative = 0;
}

    public void SaveData()
    {
        var serializer = new XmlSerializer(typeof(DataContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
            stream.Close();
        }

        Debug.Log("Saved to " + path);
    }
    public bool LoadData()
    {
        var serializer = new XmlSerializer(typeof(DataContainer));
        if (CheckFileExists())
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                DataContainer container = serializer.Deserialize(stream) as DataContainer;
                stream.Close();

                //loaded into in scope container
                //set back to appropriate objects
                SendData(container);
            }
            return true;
        }
        return false;
    }

    public bool CheckFileExists()
    {
        if (File.Exists(path)) return true;
        else return false;
    }

    public void SetData() 
    {
        currency = GameData.currency;

        maxHealth = GameData.stats.GetMaxHealth();
        maxMovementPoints = GameData.stats.GetMaxMovementPoints();
        maxActionPoints = GameData.stats.GetMaxActionPoints();
        baseInitiative = GameData.stats.GetBaseInitiative();
    }

    private void SendData(DataContainer container)
    {
        currency = container.currency;

        maxHealth = container.maxHealth;
        maxMovementPoints = container.maxMovementPoints;
        maxActionPoints = container.maxActionPoints;
        baseInitiative = container.baseInitiative;

        stats = new Stats(maxHealth,maxMovementPoints,maxActionPoints,baseInitiative);
    }
}
