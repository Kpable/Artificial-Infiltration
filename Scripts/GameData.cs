using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class GameSettings
{
    public bool fullscreen;
    public int resolution;
    public int textureQuality;
    public int antiAliasing;
    public int vSync;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public int shadowResolution;
    public bool softParticles;

    public GameSettings()
    {
        // all default values should be fine. 
    }

    public GameSettings(GameSettings settings)
    {
        fullscreen = settings.fullscreen;
        resolution = settings.resolution;
        textureQuality = settings.textureQuality;
        antiAliasing = settings.antiAliasing;
        vSync = settings.vSync;
        masterVolume = settings.masterVolume;
        musicVolume = settings.musicVolume;
        sfxVolume = settings.sfxVolume;
        shadowResolution = settings.shadowResolution;
        softParticles = settings.softParticles;
    }

}

[Serializable]
public class SaveData
{
    public List<MissionData> missionData;
    public Missions lastPlayedScene;
    public string playerModel;
    public string playerMaterial;

    public SaveData()
    {
        missionData = new List<MissionData>();
    }

    public override string ToString()
    {
        string saveData = "";
        foreach (MissionData m in missionData)
        {
            saveData += "levelName: " + m.levelName + ",";
            saveData += " levelTime: " + m.levelTime.ToString() + ",";
            saveData += " recievedDate: " + m.recievedDate.ToString() + ",";
            saveData += " unlocked: " + m.unlocked.ToString() + ",";
            saveData += " completed: " + m.completed.ToString() + "\n";

        }
        saveData += "lastPlayedScene: \n" + lastPlayedScene.ToString();
        saveData += "playerModel: " + playerModel;
        saveData += "playerMaterial: " + playerMaterial;
        return saveData;
    }
}

[Serializable]
public class MissionData
{
    public string levelName;
    public DateTime levelTime;
    public DateTime recievedDate;
    public bool unlocked;
    public bool completed;
}

public static class GameData {

    public static SaveData CreateNewSaveData(string filename)
    {
        if (File.Exists(filename))
            DeleteData(filename);

        //playerSaveGame = new List<MissionData>();
        SaveData saveGame = new SaveData();
        // For each mission in our build settings, create a new mission data. 
        for (int i = 1; i < (int)Missions.Total; i++)
        {
            MissionData mission = new MissionData();
            mission.levelName = ((Missions)i).ToString();
            mission.levelTime = DateTime.MaxValue;                      // Set to max so first time will be < default
            mission.recievedDate = DateTime.MaxValue;                   // Set to max so first time will be < default
            if (i == 1) mission.unlocked = true;                        // Unlock the first Mission          
            else mission.unlocked = false;                              // This is default value
            mission.completed = false;                                  // This is the default value

            //playerSaveGame.Add(data);
            saveGame.missionData.Add(mission);
        }

        saveGame.lastPlayedScene = (Missions)1;   // Last played scene is the first mission. 
        saveGame.playerModel = "";
        saveGame.playerMaterial = "";

        SaveData(filename, saveGame);
        return saveGame;
    }

    public static GameSettings CreateNewGameSettings(string filename)
    {
        if (File.Exists(filename))
            DeleteData(filename);

        // Set Default values
        GameSettings gameSettings = new GameSettings();
        gameSettings.fullscreen = true;             // On   
        gameSettings.resolution = -1;                // we wont know what this is until the game is ran
        gameSettings.textureQuality = 0;            // High
        gameSettings.antiAliasing = 0;              // Disabled
        gameSettings.vSync = 0;                     // None
        gameSettings.masterVolume = 0.8f;           // 80%
        gameSettings.musicVolume = 0.8f;            // 80%
        gameSettings.sfxVolume = 0.8f;              // 80%
        gameSettings.softParticles = true;          // On
        gameSettings.shadowResolution = 2;          // High

        SaveJson(filename, JsonUtility.ToJson(gameSettings, true));
        return gameSettings;
}

    #region IO - Save Load Delete
    public static void SaveData(string filename, object data)
    {
        Debug.Log("Saving Data to: " + filename);

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream saveFile = File.Create(filename);

        formatter.Serialize(saveFile, data);

        saveFile.Close();
    }

    public static object LoadData(string filename)
    {
        
        
        object data = null;

        if (File.Exists(filename))
        {
            Debug.Log("Loading Data from: " + filename);
            //Debug.Log("File Exists");

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream saveFile = File.Open(filename, FileMode.Open);

            data = formatter.Deserialize(saveFile);

            saveFile.Close();
        }
        else
        {
            Debug.LogWarning("File: " + filename + " does not exist");
        }

        return data; 
    }

    public static bool DeleteData(string filename)
    {
        if (File.Exists(filename))
        {
            File.Delete(filename);
        }
        else
        {
            Debug.LogError("File " + filename + " does not exists");

            return false;
        }

        if (File.Exists(filename))
        {
            Debug.LogError("Deletion of " + filename + " Failed");
            return false;
        }

        return true;
    }



    public static void SaveJson(string filename, string jsonData)
    {
        Debug.Log("Saving Json Data to: " + filename);

        File.WriteAllText(filename, jsonData);
    }

    public static string LoadJson(string filename)
    {
        
        string jsonData = "";
        if (File.Exists(filename))
        {
            Debug.Log("Loading Json Data to: " + filename);
            jsonData = File.ReadAllText(filename);
        }
        else
            Debug.Log("File does not Exists: " + filename);
        
        return jsonData;
    }

    #endregion
}
