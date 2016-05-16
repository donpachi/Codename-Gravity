using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Singleton Game Control class
public class GameControl
{
    private static GameControl _instance;
    public static GameControl Instance { get { return _instance ?? (_instance = new GameControl()); } }

    private bool[] levelUnlocked;
    private int[] levelHighScore;
    private int latestLevel;

    private GameControl()
    {
        if (!File.Exists(Application.persistentDataPath + "/gameSave.dat"))
        {
            NewGame();
        }
        else
        {
            Load();
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Debug.Log(System.Environment.Version);
    }
    
    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        DontDestroyOnLoad(gameObject);
    //        Instance = this;
    //    }
    //    else if (Instance != this)
    //    {
    //        Destroy(gameObject);
    //    }

    //    if (!File.Exists(Application.persistentDataPath + "/gameSave.dat"))
    //    {
    //        NewGame();
    //    }
    //    else
    //    {
    //        Load();
    //    }
    //}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameSave.dat");

        GameData data = new GameData();
        data.levelUnlocked = levelUnlocked;
        data.levelHighScore = levelHighScore;
        data.latestLevel = latestLevel;
        
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);
            try
            {
                GameData data = (GameData)bf.Deserialize(file);

                levelUnlocked = data.levelUnlocked;
                levelHighScore = data.levelHighScore;
                latestLevel = data.latestLevel;
                file.Close();
            }
            catch(TypeLoadException e)
            {
                Debug.LogError(e + " The Game had trouble loading");
                file.Close();
                NewGame();
            }
        }
    }

    public void NewGame()
    {
        levelUnlocked = new Boolean[Application.levelCount-1];
        levelUnlocked[0] = true; // Unlock First Level
        levelHighScore = new Int32[Application.levelCount-1];
        latestLevel = 1;
        Save();
    }

    public void SetLevelComplete(int score)
    {
        if (score > levelHighScore[Application.loadedLevel])
            levelHighScore[Application.loadedLevel] = score;

        if (latestLevel <= Application.loadedLevel)
            latestLevel = Application.loadedLevel+1;

        levelUnlocked[Application.loadedLevel] = true;
        Save();
    }

    public bool[] GetLevelUnlock()
    {
        return levelUnlocked;
    }

    public int[] GetLevelHighScore()
    {
        return levelHighScore;
    }

    public int GetLatestLevel()
    {
        return latestLevel;
    }

}

[Serializable]
class GameData
{
    public bool[] levelUnlocked;
    public int[] levelHighScore;
    public int latestLevel;
}
