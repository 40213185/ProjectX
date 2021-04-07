using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DataContainer
{
    //container data
    public int currency;


    //container methods
    string path = Application.persistentDataPath + "/GameData.dat";

    public DataContainer() 
    {
        currency = 0;
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
    public void LoadData()
    {
        var serializer = new XmlSerializer(typeof(DataContainer));
        if (CheckFileExists())
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                DataContainer container = serializer.Deserialize(stream) as DataContainer;
                stream.Close();
                SetData(container);
            }
        }
        else SaveData();
    }

    public bool CheckFileExists()
    {
        if (File.Exists(path)) return true;
        else return false;
    }

    public void SetData(DataContainer dataContainer) 
    {
        currency = GameData.currency;
    }
}
