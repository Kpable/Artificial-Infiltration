using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

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
