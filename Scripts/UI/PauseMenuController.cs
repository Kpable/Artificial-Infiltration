using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MenuController {
    public Button pauseButton;

    public Button resumeButton, mainMenuButton, restartButton, optionsButton;

    [Header("Sub Menus")]
    public GameObject optionsMenu;

    private AudioSource audioSource;

    void Awake()
    {
        // Get or add the audio for the sound effect.
        audioSource = GetComponentInChildren<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning(name + " Cannot find audio source to play sound effects. Adding it.");
            transform.Find("SFX").gameObject.AddComponent<AudioSource>();
            audioSource = GetComponentInChildren<AudioSource>();
        }
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();

        // Pause Button
        pauseButton.onClick.AddListener(delegate { StartCoroutine("PauseGame"); });

        // Pause Menu Buttons
        resumeButton.onClick.AddListener(delegate { StartCoroutine("ResumeGame"); });
        mainMenuButton.onClick.AddListener(delegate { MainMenu(); });
        restartButton.onClick.AddListener(delegate { Restart(); });
        optionsButton.onClick.AddListener(delegate { Options(); });
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine("PauseGame");
        }
    }

    public IEnumerator PauseGame()
    {
        AudioClip clip = null;
        if (!menuPanel.activeInHierarchy)
        {
            // Play pause on Sound effect
            clip = SoundManager.instance.GetSoundEffect("Pause On");
            if (clip != null)
                audioSource.PlayOneShot(clip);


            // Open the pause menu
            Utils.StopTime();
            ShowBackgroundImage();
            yield return new WaitForSecondsRealtime(fadeDuration);
            ShowMenuPanel();

            // Show Cursor
            Cursor.visible = true;

        }
        else
        {
            // Play pause off sound effect
            clip = SoundManager.instance.GetSoundEffect("Pause Off");
            if (clip != null)
                audioSource.PlayOneShot(clip);

            // Close the pause menu
            HideMenuPanel();
            yield return new WaitForSecondsRealtime(fadeDuration);
            HideBackgroundImage();
            yield return new WaitForSecondsRealtime(fadeDuration);
            Utils.StartTime();

            // Hide cursor again
            Cursor.visible = false;

        }
    }

    #region Pause Menu Buttons

    public IEnumerator ResumeGame()
    {
        HideMenuPanel();
        yield return new WaitForSecondsRealtime(fadeDuration);
        HideBackgroundImage();
        yield return new WaitForSecondsRealtime(fadeDuration);
        Utils.StartTime();

        // Hide cursor again
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        // Start time again. 
        Utils.StartTime();
        // Reload the first scene
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        // Start time again. 
        Utils.StartTime();
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void Options()
    {
        // Hide Main Menu
        HideMenuPanel();
        // Show Options Menu
        optionsMenu.SetActive(true);
        optionsMenu.GetComponent<MenuController>().StartCoroutine("ShowMenu");
    }

    #endregion End Pause Menu Buttons



}
