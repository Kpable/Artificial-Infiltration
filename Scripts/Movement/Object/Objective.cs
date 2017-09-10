using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour {


    [SerializeField]
    private InGameTimer timer;          // The timer so this objective can stop it. 

    private bool missionRunner;         // If we are in Mission Runner mode

    private GameManager gameManager;
    private GameObject missionEndMenu;

    void Start()
    {
        gameManager = GameManager.instance;
        if (gameManager) // should only be null in development but better safe.. yadayada
            missionRunner = gameManager.missionRunnerMode;

        missionEndMenu = GameObject.Find("Mission End Menu");
        if (missionEndMenu == null) Debug.LogError(name + ": Cannot find Mission End Menu");

        if (!timer) timer = GameObject.Find("Timer Text").GetComponent<InGameTimer>();
        if (!timer && missionRunner) Debug.LogError(name + ": Unable to save best time if no timer is found ");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            // Pause the game. 
            Utils.StopTime();
            // If Mission Runner mode
            if (missionRunner)
            {
                if (gameManager && timer) gameManager.HandleScore(SceneManager.GetActiveScene().buildIndex, timer.GetTime());

                // Return to main menu
                SceneManager.LoadScene(0);  // Load Main menu

            }
            // Else
            else
            {
                // Save mission completion
                if(gameManager) gameManager.CompleteMission(SceneManager.GetActiveScene().buildIndex);
                // Refresh Mission List Meu
                if(missionEndMenu) missionEndMenu.GetComponent<MissionEndMenuController>().RefreshMissionEndStatus();
                // Open Mission List Menu
                if (missionEndMenu) missionEndMenu.GetComponent<MenuController>().StartCoroutine("ShowMenu");

            }
        }
    }
}
