using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MenuController {

    [Header("Main Menu")]
    public Button newGameButton;
    public Button loadGameButton, missionRunnerButton, optionsButton, exitButton, creditsButton, playerCustomizationButton;

    [Header("Sub Menus")]
    public GameObject newGameMenu;
    public GameObject missionRunnerMenu;
    public GameObject optionsMenu;
    public GameObject missionListMenu;
    public GameObject playerCustomizationMenu;

    private GameManager gameManager;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        // Delegating each onClick listener action to a function that will do the desired action for each button

        // Main Menu Buttons
        newGameButton.onClick.AddListener( delegate { NewGame(); });
        loadGameButton.onClick.AddListener( delegate { LoadGame(); });
        missionRunnerButton.onClick.AddListener(delegate { MissionRunner(); });
        optionsButton.onClick.AddListener( delegate { Options(); });
        exitButton.onClick.AddListener( delegate { Exit(); });
        creditsButton.onClick.AddListener( delegate { Credits(); });
        playerCustomizationButton.onClick.AddListener( delegate { PlayerCustomization(); });

        gameManager = GameManager.instance;

    }

    #region Main Menu Buttons

    public void NewGame()
    {
        // Hide Main Menu 
        HideMenuPanel();
        // Show Mission Runner Menu
        newGameMenu.SetActive(true);
        newGameMenu.GetComponent<MenuController>()
            .Invoke("ShowBackgroundImage", fadeDuration);
        //TODO Show menu panel at target menus fade duration
        newGameMenu.GetComponent<MenuController>()
            .Invoke("ShowMenuPanel", fadeDuration * 2);
    }

    public void LoadGame()
    {
        int nextMission = 1;
        for (int i = 1; i < (int)Missions.Total; i++)
        {
            if (!gameManager.playerSaveGame.missionData[i - 1].completed)
            {
                // If this is the first mission and it is not yet completed
                if (i == 1) nextMission = i;
                // If mission not completed and this is not the first mission. 
                else if (i > 1)
                    // If this mission is unlocked and the previous mission is completed
                    if (gameManager.playerSaveGame.missionData[i - 1].unlocked &&
                        gameManager.playerSaveGame.missionData[i - 2].completed)
                        nextMission = i;
            }
        }

        SceneManager.LoadScene(nextMission);

        //// Hide Main Menu 
        //HideMenuPanel();
        //// Show Mission Runner Menu
        //missionListMenu.SetActive(true);
        //missionListMenu.GetComponent<MenuController>()
        //    .Invoke("ShowBackgroundImage", fadeDuration);
        ////TODO Show menu panel at target menus fade duration
        //missionListMenu.GetComponent<MenuController>()
        //    .Invoke("ShowMenuPanel", fadeDuration * 2);

        // Check Save Data for last played Scene
        // Load that scene
        //gameManager.missionRunnerMode = false;
        //gameManager.missionLoaded = true;
        //SceneManager.LoadScene((int)playerSavedData.lastPlayedScene);
    }

    public void MissionRunner()
    {
        // Hide Main Menu 
        HideMenuPanel();
        // Show Mission Runner Menu
        missionRunnerMenu.SetActive(true);
        missionRunnerMenu.GetComponent<MenuController>()
            .Invoke("ShowBackgroundImage", fadeDuration);
        //TODO Show menu panel at target menus fade duration
        missionRunnerMenu.GetComponent<MenuController>()
            .Invoke("ShowMenuPanel", fadeDuration * 2);
    }

    public void Options()
    {
        // Hide Main Menu
        HideMenuPanel();
        // Show Options Menu
        optionsMenu.SetActive(true);
        optionsMenu.GetComponent<MenuController>()
            .Invoke("ShowBackgroundImage", fadeDuration);
        //TODO Show menu panel at target menus fade duration
        optionsMenu.GetComponent<MenuController>()
            .Invoke("ShowMenuPanel", fadeDuration * 2);
    }

    public void Exit()
    {
        // Quit the Game
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void PlayerCustomization()
    {
        // Hide Main Menu
        HideMenuPanel();
        // Show Options Menu
        playerCustomizationMenu.SetActive(true);
        playerCustomizationMenu.GetComponent<MenuController>()
            .Invoke("ShowBackgroundImage", fadeDuration);
        //TODO Show menu panel at target menus fade duration
        playerCustomizationMenu.GetComponent<MenuController>()
            .Invoke("ShowMenuPanel", fadeDuration * 2);
    }

    #endregion End Main Menu Buttons


}
