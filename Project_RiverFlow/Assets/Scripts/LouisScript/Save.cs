using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Save : MonoBehaviour
{

    
    public static void SaveLevel(List<LevelSave> LevelSaves)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.bin";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, LevelSaves);
        stream.Close();
    }

    public static List<LevelSave> LoadSave()
    {
        string path = Application.persistentDataPath + "/save.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<LevelSave> saves = formatter.Deserialize(stream) as List<LevelSave>;
            stream.Close();

            return saves;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
}
