using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenuController : MenuController {

    [Header("Menu")]
    public Button saveButton;

    [Header("Audio")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Video")]
    public Toggle fullscreenToggle;
    public Toggle softParticlesToggle;
    public Dropdown resolutionDropdown, textureQualityDropdown, antiAliasingDropdown, vSyncDropdown, shadowResolutionDropdown;

    private GameManager gameManager;
    //private SaveData playerSavedData;
    private GameSettings gameSettings;
    private Resolution[] resolutions;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        gameManager = GameManager.instance;
        //playerSavedData = gameManager.playerSaveGame;

        masterVolumeSlider.onValueChanged.AddListener(delegate { MasterVolume(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { MusicVolume(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { SFXVolume(); });

        fullscreenToggle.onValueChanged.AddListener(delegate { FullScreen(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { Resolution(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { TextureQuality(); });
        antiAliasingDropdown.onValueChanged.AddListener(delegate { AntiAliasing(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { VSync(); });
        softParticlesToggle.onValueChanged.AddListener(delegate { SoftParticles(); });
        shadowResolutionDropdown.onValueChanged.AddListener(delegate { ShadowResolution(); });


        saveButton.onClick.AddListener(delegate { Save(); });


        resolutions = Screen.resolutions;
        resolutionDropdown.options.Clear();
        foreach (Resolution r in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(r.ToString()));
        }

        //Reset the settings after resolutions list is populated
        ResetSettings();

        // gameSettings will be updated with ResetSettings. 
        // This sets the dropdown to the right value.
        // -1 is the default value. We dont want to set it to the lowest resolution first time the game is ran
        if(gameSettings.resolution != -1)
            resolutionDropdown.value = gameSettings.resolution;

        // Refresh resolution dropdown. 
        resolutionDropdown.RefreshShownValue();

    }

    #region Audio

    public void MasterVolume()
    {
        gameSettings.masterVolume = masterVolumeSlider.value;

        SoundManager.instance.AdjustMasterVolume(gameSettings.masterVolume);
        SoundManager.instance.AdjustSFXVolume(gameSettings.sfxVolume);
        SoundManager.instance.AdjustMusicVolume(gameSettings.musicVolume);

    }

    public void MusicVolume()
    {
        gameSettings.musicVolume = musicVolumeSlider.value;

        SoundManager.instance.AdjustMusicVolume(gameSettings.musicVolume);
    }

    public void SFXVolume()
    {
        gameSettings.sfxVolume = sfxVolumeSlider.value;

        SoundManager.instance.AdjustSFXVolume(gameSettings.sfxVolume);

    }

    #endregion

    #region Video

    public void FullScreen()
    {
        Screen.fullScreen = gameSettings.fullscreen = fullscreenToggle.isOn;
    }

    public void Resolution()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, 
            resolutions[resolutionDropdown.value].height, 
            Screen.fullScreen);
        gameSettings.resolution = resolutionDropdown.value;

        Camera.main.fieldOfView = Mathf.Round(60 * Utils.DesiredAspectRatio / ((float)Camera.main.pixelWidth / Camera.main.pixelHeight));
        
    }

    public void TextureQuality()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;

    }

    public void AntiAliasing()
    {
        QualitySettings.antiAliasing = gameSettings.antiAliasing = (int)Mathf.Pow(2f, antiAliasingDropdown.value);
        Debug.Log("resolution value: " + gameSettings.antiAliasing);

    }

    public void VSync()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;

    }

    public void SoftParticles()
    {
        QualitySettings.softParticles = gameSettings.softParticles = softParticlesToggle.isOn;
    }

    public void ShadowResolution()
    {
        gameSettings.shadowResolution = shadowResolutionDropdown.value;
        QualitySettings.shadowResolution = (ShadowResolution)gameSettings.shadowResolution;
    }

    #endregion

    public void Save()
    {
        // Save the Settings
        GameData.SaveJson(Utils.GameSettingsFilename, JsonUtility.ToJson(gameSettings, true));
        //Update the Game Manager of the new settings. 
        gameManager.gameSettings = new GameSettings(gameSettings);

        // Adjust the field of view to show the same amount. 
        Camera.main.fieldOfView = Mathf.Round(60 * Utils.DesiredAspectRatio / ((float)Camera.main.pixelWidth / Camera.main.pixelHeight));

        Back();
    }

    public void LoadGameSettings()
    {
        // The Game Manager has already loaded them on game start, so get the settings from it.
        // TODO this gets the manager's instance instead of copying its values. 
        gameSettings = new GameSettings(gameManager.gameSettings);

        // Adjust all UI Elements to show the current settings. 
        masterVolumeSlider.value = gameSettings.masterVolume;
        musicVolumeSlider.value = gameSettings.musicVolume;
        sfxVolumeSlider.value = gameSettings.sfxVolume;

        fullscreenToggle.isOn = gameSettings.fullscreen;
        if(gameSettings.resolution != -1)
            resolutionDropdown.value = gameSettings.resolution;
        textureQualityDropdown.value = gameSettings.textureQuality;
        antiAliasingDropdown.value = gameSettings.antiAliasing;
        vSyncDropdown.value = gameSettings.vSync;
        softParticlesToggle.isOn = gameSettings.softParticles;
        shadowResolutionDropdown.value = gameSettings.shadowResolution;
        
        // Refresh resolution dropdown. 
        resolutionDropdown.RefreshShownValue();
    }

    public void ResetSettings()
    {
        Debug.Log("Resetting Settings");
        LoadGameSettings();

        SoundManager.instance.AdjustMasterVolume(gameSettings.masterVolume);
        SoundManager.instance.AdjustSFXVolume(gameSettings.sfxVolume);
        SoundManager.instance.AdjustMusicVolume(gameSettings.musicVolume);

        Screen.fullScreen = gameSettings.fullscreen;
        if (gameSettings.resolution != -1)
        {
            Screen.SetResolution(resolutions[resolutionDropdown.value].width,
               resolutions[resolutionDropdown.value].height,
               Screen.fullScreen);
        }
        else
        {   // since we dont know the computer's resolutions, set it to the highest found
            Screen.SetResolution(resolutions[resolutions.Length - 1].width,
               resolutions[resolutions.Length - 1].height,
               Screen.fullScreen);
            // Lets save this since we just set it. 
            gameSettings.resolution = resolutions.Length - 1;
            Save();
        }
        QualitySettings.masterTextureLimit = gameSettings.textureQuality;
        QualitySettings.antiAliasing = gameSettings.antiAliasing;
        QualitySettings.vSyncCount = gameSettings.vSync;
        QualitySettings.softParticles = gameSettings.softParticles;
        QualitySettings.shadowResolution = (ShadowResolution) gameSettings.shadowResolution;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Back()
    {
        Debug.Log("Options Menu Back");
        // IF the settings are being discarded, reset them before closing the menu. 
        if (gameSettings != gameManager.gameSettings)
        {
            Debug.Log("Different GameSettings");

            ResetSettings();
        }
        
        base.Back();
    }
}
