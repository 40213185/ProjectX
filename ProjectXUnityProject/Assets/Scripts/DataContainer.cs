using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DataContainer
{
    string path = Application.persistentDataPath + "/GameData.dat";

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
        using (var stream = new FileStream(path, FileMode.Open))
        {
            DataContainer container = serializer.Deserialize(stream) as DataContainer;
            stream.Close();
            SetData(container);
        }
    }

    public bool CheckFileExists()
    {
        if (File.Exists(path)) return true;
        else return false;
    }

    public void SetData(DataContainer dataContainer) 
    {

    }
}
