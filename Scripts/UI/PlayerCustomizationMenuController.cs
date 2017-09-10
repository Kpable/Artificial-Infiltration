using UnityEngine;
using UnityEngine.UI;

public class PlayerCustomizationMenuController : MenuController {

    public Button saveButton;
    public ItemSlider /*playerModelItemSlider,*/ playerMaterialItemSlider;
    //public List<PlayerModel> models = new List<PlayerModel>();
    public GameObject playerModel;
    //public List<PlayerMaterial> materials = new List<PlayerMaterial>();

    private GameManager gameManager;
    private GameObject activeModel;

    // Use this for initialization
    override protected void Start ()
    {
        base.Start();

        gameManager = GameManager.instance;

        saveButton.onClick.AddListener(delegate { Save(); });
        Initialize();

    }

    private void Initialize()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform models = player.transform.Find("Models");
        //activeModel = models.GetChild(0).gameObject;
        activeModel = playerModel;
        

        //activeModel = GameObject.FindGameObjectWithTag("Player").transform.FindChild("Models").GetChild(0).gameObject;

        //foreach (PlayerModel model in models)
        //{
        //    if (playerModelItemSlider) playerModelItemSlider.AddItem(model.name);

        //    if (gameManager.playerSaveGame.playerModel.Equals(model.name))
        //    {
        //        // Set it as the active model
        //        activeModel = model.model;
        //        // Set the UI Text 
        //        playerModelItemSlider.SetText(model.name);
        //        // Activate the model
        //        model.model.SetActive(true);
        //    }
        //    else
        //    {
        //        // Deactivate all other models
        //        model.model.SetActive(false);
        //    }
        //}

        foreach (PlayerMaterial material in gameManager.materials)
        {
            if (playerMaterialItemSlider) playerMaterialItemSlider.AddItem(material.name);
        }

        SetUI();        

        // Save the material and model to the initial values if we had to set them. 
        // Will save the already saved values otherwise. 
        gameManager.SavePlayerCustomization(gameManager.playerSaveGame.playerModel, gameManager.playerSaveGame.playerMaterial);
    }

    private void SetUI()
    {
        foreach (PlayerMaterial material in gameManager.materials)
        {
            if (gameManager.playerSaveGame.playerMaterial.Equals(material.name))
            {
                //if (activeModel.GetComponent<Renderer>()) 
                //    activeModel.GetComponent<Renderer>().material = material.material;
                if (activeModel.GetComponentInChildren<Renderer>())
                    activeModel.GetComponentInChildren<Renderer>().material = material.material;

                // Set the UI Text to the material in the save file. 
                playerMaterialItemSlider.SetText(material.name);
            }
        }

        // Set save data to defaults if it wasnt set already. 
        //if (gameManager.playerSaveGame.playerModel.Equals(""))
        //{
        //    // Set the active model to the first model
        //    activeModel = models[0].model;
        //    // Save that model name
        //    gameManager.playerSaveGame.playerModel = models[0].name;
        //    // Set the UI Text to the model name
        //    playerModelItemSlider.SetText(models[0].name);
        //}

        if (gameManager.playerSaveGame.playerMaterial.Equals(""))
        {
            //if (activeModel.GetComponent<Renderer>())
            //    activeModel.GetComponent<Renderer>().material = materials[0].material;
            if (activeModel.GetComponentInChildren<Renderer>())
                activeModel.GetComponentInChildren<Renderer>().material = gameManager.materials[0].material;
            // Save that material name
            gameManager.playerSaveGame.playerMaterial = gameManager.materials[0].name;
            // Set the UI Text
            playerMaterialItemSlider.SetText(gameManager.materials[0].name);
        }
    }

    public void SliderValueChanged()
    {
        //foreach (PlayerModel model in models)
        //{
        //    if (playerModelItemSlider.GetText().Equals(model.name))
        //    {
        //        model.model.SetActive(true);
        //        activeModel = model.model;
        //    }
        //    else
        //    {
        //        model.model.SetActive(false);
        //    }
        //}

        foreach (PlayerMaterial material in gameManager.materials)
        {
            if (playerMaterialItemSlider.GetText().Equals(material.name))
                if (activeModel)
                {
                    //if(activeModel.GetComponent<Renderer>())
                    //    activeModel.GetComponent<Renderer>().material = material.material;
                    if(activeModel.GetComponentInChildren<Renderer>())
                        activeModel.GetComponentInChildren<Renderer>().material = material.material;
                }
        }
    }

    // Update is called once per frame
    override protected void Update () {
        base.Update();
	}

    public void Save()
    {
        // Save the Settings
        gameManager.SavePlayerCustomization("", playerMaterialItemSlider.GetText());
        
        Back();
    }

    protected override void Back()
    {
        SetUI();

        base.Back();
    }
}

[System.Serializable]
public class PlayerModel
{
    public string name;
    public GameObject model;
}

[System.Serializable]
public class PlayerMaterial
{
    public string name;
    public Material material;
}
