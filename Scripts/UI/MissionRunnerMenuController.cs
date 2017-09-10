using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionRunnerMenuController : MenuController {

    public GameObject missionRunnerMissionButtonPrefab;

    [SerializeField]
    private Transform missionRunnerItemParent;

    private GameManager gameManager;
    private SaveData playerSavedData;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        gameManager = GameManager.instance;
        playerSavedData = gameManager.playerSaveGame;

        // Mission Runner Buttons
        for (int i = 1; i < (int)Missions.Total; i++)
        {
            // Create a new mission runner button. 
            GameObject btn = Instantiate(missionRunnerMissionButtonPrefab) as GameObject;
            // Set its parent to be the panel, false to disable world position and get canvas position. 
            btn.transform.SetParent(missionRunnerItemParent, false);
            // Find the Button's Text to set it to the mission name. 
            btn.transform.Find("Mission Button").GetComponentInChildren<Text>().text =
                ((Missions)i).ToString().Replace("_", " ");
            // Does the same as above but grabs it from player save data
            //btn.transform.FindChild("Mission Button").GetComponentInChildren<Text>().text = 
            //    playerSavedData.missionData[i - 1].levelName;
            // Grab the time score text and set that to this mission's best time. 
            btn.transform.Find("Best Time").GetComponent<Text>().text =
                playerSavedData.missionData[i - 1].levelTime.PrettyTime();

            //            Debug.Log(playerSavedData.ToString());
            // If mission is locked in campaign, lock it in Mission Runner mode. 
            if (!playerSavedData.missionData[i - 1].unlocked)
            {
                btn.transform.Find("Mission Button").GetComponentInChildren<Button>().interactable = false;
            }

            //Debug.Log("LoadMissionDelegate " + i);

            btn.transform.Find("Mission Button").GetComponentInChildren<Button>()
                .onClick.AddListener(delegate { LoadMission(); });
        }
    }

    public void LoadMission()
    {
        // Figure out what button pressed this. 
        GameObject pressedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        // Get the text name. 
        string missionName = pressedButton.GetComponentInChildren<Text>().text;

        int sceneNumber = 1;
        // Compare the mission name to our enum names and return the index as the scene number. 
        for (int i = 0; i < (int)Missions.Total; i++)
        {
            if (missionName == ((Missions)i).ToString().Replace("_", " "))
                sceneNumber = i;
        }
        gameManager.missionRunnerMode = true;
        SceneManager.LoadScene(sceneNumber);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
