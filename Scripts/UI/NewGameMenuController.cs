using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameMenuController : MenuController {


    [Header("New Game")]
    public Button confirmButton;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        // New Game Buttons
        confirmButton.onClick.AddListener(delegate { Confirm(); });
    }
	
    #region New Game Buttons

    public void Confirm()
    {
        // Erase Game Data
        // Save new Game Data. 
        GameData.CreateNewSaveData(Utils.GetPlayerSaveFilePath());

        // Load first mission. 
        SceneManager.LoadScene(1);
    }

    #endregion End New Game Buttons
}
