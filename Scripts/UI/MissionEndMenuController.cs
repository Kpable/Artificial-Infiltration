using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionEndMenuController : MenuController {

    public Button confirmButton;

    private GameManager gameManager;
    private SaveData playerSavedData;
    private int nextMission;

    protected override void Start()
    {
        base.Start();
        gameManager = GameManager.instance;
        playerSavedData = gameManager.playerSaveGame;
        RefreshMissionEndStatus();


        // Buttons
        confirmButton.onClick.AddListener(delegate { Confirm(); });
    }

    public void RefreshMissionEndStatus()
    {


        for (int i = 1; i < (int)Missions.Total; i++)
        {
            // Completed misisons have secondary text loaded instead of primary. 
            if (!playerSavedData.missionData[i - 1].completed)
            {
                // If this is the first mission and it is not yet completed
                if (i == 1)
                {
                    nextMission = i;
                }
                // If mission not completed and this is not the first mission. 
                else if (i > 1)
                {
                    // Remember missionData is 0 index based whereas the mission in 
                    // everywhere else is build index based so starting at 1

                    // If this mission is unlocked and the previous mission is completed
                    if (playerSavedData.missionData[i - 1].unlocked && playerSavedData.missionData[i - 2].completed)
                    {
                        nextMission = i;
                    }
                }

            }
        }
    }


    public void Confirm()
    {
        // Load next mission. 
        SceneManager.LoadScene(nextMission);

    }
}
