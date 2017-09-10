using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum Missions
{
    MainMenu,

    Mission_1, 
    Mission_2, 
    Mission_3, 
    Mission_4,
    Mission_5,

    Total // also the Credits Scene
}

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public SaveData playerSaveGame = new SaveData();
    public GameSettings gameSettings = new GameSettings();
    public List<PlayerMaterial> materials = new List<PlayerMaterial>();


    [HideInInspector]
    public bool missionRunnerMode;
    [HideInInspector]
    public bool missionLoadedFromSave;

    private void Awake()
    {
        MakeSingleton();
    }

    private void MakeSingleton()
    {
        // Make a Singleton
        // If the static instance isnt null
        if (instance != null)
        {
            //We already have one Game Manager Object In the Game, Destroy all others
            Destroy(gameObject);
        }
        else
        {
            //This is the first instance of Game Manager, Store it and prevent it from dying. 
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Only Initialize once
            Initialize();
        }
    }

    private void Initialize()
    {
        // Check if there are game settings saved
        gameSettings = JsonUtility.FromJson<GameSettings>(GameData.LoadJson(Utils.GetSettingsFilePath()));
        if(gameSettings == null)
        {
            // If not create the game settings with default values. 
            gameSettings = GameData.CreateNewGameSettings(Utils.GetSettingsFilePath());
        }
        Debug.Log("GameSettings " + gameSettings.ToString());

        playerSaveGame = (SaveData)GameData.LoadData(Utils.GetPlayerSaveFilePath());
        // Check if there is save data
        if (playerSaveGame == null)
        {
            // If not create the save data
            playerSaveGame = GameData.CreateNewSaveData(Utils.GetPlayerSaveFilePath());
           
        }
        Debug.Log("PlayerSaveGame\n" + playerSaveGame.ToString());

        // Unregister as awake is called every time a scene is loaded. 
        SceneManager.sceneLoaded -= OnMissionLoad;
        // Then add to avoid calling the same method multiple times.. It causes problems. 
        SceneManager.sceneLoaded += OnMissionLoad;

    }

    private void OnMissionLoad(Scene mission, LoadSceneMode mode)
    {
        Debug.Log("Load Mode: " + mode);
        if (!missionLoadedFromSave && Time.timeScale == 0)
            Time.timeScale = 1;     // If time was stopped start it 

        if (mission.buildIndex == 0 && missionRunnerMode)
        {
            // We've returned to tha main menu with missionRunnerMode enabled
            // We just completed a mission runner mission
            // Set the mode back to normal
            missionRunnerMode = false;

            // Tell the main menu to jump to the mission runner screen. 
            GameObject missionRunnerMenu = GameObject.Find("Mission Runner Menu");
            if(missionRunnerMenu) missionRunnerMenu.GetComponent<MenuController>().StartCoroutine("ShowMenu");
        }

        if(mission.buildIndex != 0 && mission.buildIndex != (int) Missions.Total )
        {
            Debug.Log("Mission: " + mission.name);

            //Hide Cursor
            Cursor.visible = false;

            GameObject timerText = GameObject.Find("Timer Text");
            if(timerText) timerText.SetActive(missionRunnerMode);
            GameObject bestTimeText = GameObject.Find("Best Time Text");
            if(bestTimeText) bestTimeText.SetActive(missionRunnerMode);
            GameObject storyTriggers = GameObject.Find("Story Triggers");
            if (storyTriggers) storyTriggers.SetActive(!missionRunnerMode);

            if (missionLoadedFromSave)
            {
                GameObject.Find("Pause Menu").GetComponent<PauseMenuController>().StartCoroutine( "PauseGame" );
                missionLoadedFromSave = false;
            }

            // Locate the Player Object
            GameObject player = GameObject.Find("Player");
            foreach (PlayerMaterial material in materials)
            {
                if(material.name == playerSaveGame.playerMaterial)
                {
                    player.transform.Find("Models").
                        GetChild(0).gameObject.GetComponentInChildren<Renderer>()
                        .material = material.material;
                }
            }

            //// Make a list to hold all the children
            //List<GameObject> playerSkins = new List<GameObject>();
            //// Go through each child object and add it to the list. 
            //for (int i = 0; i < player.transform.childCount; i++)
            //{
            //    playerSkins.Add(player.transform.FindChild("Models").GetChild(i).gameObject);
            //}

            //foreach (GameObject model in playerSkins)
            //{
            //    if (model.name.Equals(playerSaveGame.playerModel))
            //        model.SetActive(true);
            //    else
            //        model.SetActive(false);
            //}

        }
        else if(mission.buildIndex == 0 && mission.buildIndex == (int)Missions.Total)
        {
            if (!Cursor.visible) Cursor.visible = true;
        }

        // Adjust the field of view to show the same amount. 
        Camera.main.fieldOfView = Mathf.Round(60 * Utils.DesiredAspectRatio / ((float)Camera.main.pixelWidth / Camera.main.pixelHeight));
    }    

    public void CompleteMission(int mission)
    {
        // Remember missionData is 0 index based whereas the mission in 
        // everywhere else is build index based so starting at 1

        // Mark current mission as completed. 
        playerSaveGame.missionData[mission - 1].completed = true;
        // Unlock the next mission, if there is one
        if (mission < playerSaveGame.missionData.Count)
            playerSaveGame.missionData[mission].unlocked = true;
        // Save new unlocked missions
        GameData.SaveData(Utils.GetPlayerSaveFilePath(), playerSaveGame);
    }

    public void HandleScore(int mission, DateTime time)
    {
        // Remember missionData is 0 index based whereas the mission in 
        // everywhere else is build index based so starting at 1

        // Check time, if time < best time for mission
        if (time < playerSaveGame.missionData[mission - 1].levelTime)
        {
            playerSaveGame.missionData[mission - 1].levelTime = time;
            playerSaveGame.missionData[mission - 1].recievedDate = DateTime.Now;
        }

        //Save new time
        GameData.SaveData(Utils.GetPlayerSaveFilePath(), playerSaveGame);
    }

    public void SavePlayerCustomization (string model, string material)
    {
        playerSaveGame.playerModel = model;
        playerSaveGame.playerMaterial = material;
        //Save the player customization
        GameData.SaveData(Utils.GetPlayerSaveFilePath(), playerSaveGame);

    }
}
