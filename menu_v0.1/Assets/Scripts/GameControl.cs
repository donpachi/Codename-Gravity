using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Singleton Game Control class
public class GameControl : MonoBehaviour {

    public static GameControl Instance;
    
    public bool[] levelUnlocked;
    public int[] levelHighScore;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameSave.dat");

        PlayerData data = new PlayerData();
        data.levelUnlocked = levelUnlocked;
        data.levelHighScore = levelHighScore;
        
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            levelUnlocked = data.levelUnlocked;
            levelHighScore = data.levelHighScore;
        }
    }
}

[Serializable]
class PlayerData
{
    public bool[] levelUnlocked;
    public int[] levelHighScore;
}
