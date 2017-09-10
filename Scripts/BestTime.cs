using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Gets attached to a text object in the level to load from the best time for this level from save file and display it.
/// </summary>
public class BestTime : MonoBehaviour {

    private GameManager gameManager; 
    private DateTime bestTime;

    private void Start()
    {
        gameManager = GameManager.instance;
        // Remember missionData is 0 index based whereas the mission in 
        // everywhere else is build index based so starting at 1
        bestTime = gameManager.playerSaveGame.missionData[SceneManager.GetActiveScene().buildIndex- 1].levelTime;

        gameObject.GetComponent<Text>().text = bestTime.PrettyTime();
    }
}
