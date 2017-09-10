using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionListMenuController : MenuController {

    public Button confirmButton, denyButton;

    public GameObject missionStatusItem;

    [SerializeField]
    private Transform missionStatusItemParent;
     
    private GameManager gameManager;
    private SaveData playerSavedData;
    private int nextMission;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();

        gameManager = GameManager.instance;
        playerSavedData = gameManager.playerSaveGame;

        // Mission List Items

        RefreshMissionItemList();

        // Buttons
        confirmButton.onClick.AddListener(delegate { Confirm(); });
        denyButton.onClick.AddListener(delegate { Deny(); });
    }

    public void RefreshMissionItemList()
    {
        // Clear all Items
        foreach(Transform item in missionStatusItemParent)
        {
            Destroy(item.gameObject);
        }

        for (int i = 1; i < (int)Missions.Total; i++)
        {
            // Create a new mission status item. 
            GameObject item = Instantiate(missionStatusItem) as GameObject;

            // Set its parent to be the designated parent, false to disable world position and get canvas position. 
            if (missionStatusItemParent != null)
                item.transform.SetParent(missionStatusItemParent, false);
            else
                Debug.LogError(name + " Cannot set mission status items properly without the designated parent.");

            // Find the Button's Text to set it to the mission name. 
            item.transform.Find("Mission Status Name").GetComponentInChildren<Text>().text =
                ((Missions)i).ToString();


            // Completed misisons have secondary text loaded instead of primary. 
            if (playerSavedData.missionData[i - 1].completed)
            {
                item.transform.Find("Primary").gameObject.SetActive(false);
                item.transform.Find("Secondary").gameObject.SetActive(true);
            }
            else // Misison not completed. 
            {
                item.transform.Find("Primary").gameObject.SetActive(true);
                item.transform.Find("Secondary").gameObject.SetActive(false);

                // If this is the first mission and it is not yet completed
                if (i == 1)
                {
                    // Lets turn on the overlay. 
                    item.transform.Find("Overlay").gameObject.SetActive(true);
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
                        // Then this is the upcoming mission. Lets turn on the overlay image. 
                        item.transform.Find("Overlay").gameObject.SetActive(true);
                        nextMission = i;

                    }
                }

            }
        }
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    public void Confirm()
    {
        // Load next mission. 
        SceneManager.LoadScene(nextMission);

    }

    public void Deny()
    {

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // Load Main Menu Scene
            SceneManager.LoadScene(0);
        }
    }
}
