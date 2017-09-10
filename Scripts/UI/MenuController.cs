using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [Tooltip("The menu background image")]
    public GameObject backgroundImage;
    [Tooltip("The menu panel")]
    public GameObject menuPanel;
    [Tooltip("The OPTIONAL back button")]
    public Button backButton;
    [Tooltip("The OPTIONAL parent menu")]
    public GameObject parentMenu;


    [Header("Settings")]
    [Tooltip("The duration of all fade animations")]
    public float fadeDuration = 0.5f;
    [Tooltip("Whether the menu times out and deactivates")]
    public bool menuTimesOut = false;
    [Tooltip("The time in seconds to wait before disabling menus")]
    public float menuIdleTimeout = 5f;


    protected float fadeSpeed;                // The speed of the animation, always 1/ fadeDuration.
    protected bool menuActive = false;        // Whether the menu is currently active.     
    protected bool deactivating = false;      // Whether we are currently deactivating menus. 
    protected float idleCountDown;            // The amount of time to wait before deactiving menus 

    public IEnumerator ShowMenu()
    {
        ShowBackgroundImage();
        yield return new WaitForSecondsRealtime(fadeDuration);
        ShowMenuPanel();
        if (!Cursor.visible) Cursor.visible = true;
    }

    public IEnumerator HideMenu()
    {
        HideMenuPanel();
        yield return new WaitForSecondsRealtime(fadeDuration);
        HideBackgroundImage();
    }

    public void ShowBackgroundImage()
    {
        StartCoroutine("EnableMenuItem", backgroundImage);
    }

    public void HideBackgroundImage()
    {
        StartCoroutine("DisableMenuItem", backgroundImage);
    }

    public void ShowMenuPanel()
    {
        StartCoroutine("EnableMenuItem", menuPanel);
    }

    public void HideMenuPanel()
    {
        StartCoroutine("DisableMenuItem", menuPanel);
    }

    IEnumerator EnableMenuItem(GameObject menuItem)
    {
        if(!menuActive) menuActive = true;
        yield return new WaitForSecondsRealtime(0);         // Dont wait to enable and play animation. 
        menuItem.SetActive(true);
        menuItem.GetComponent<Animator>().speed = fadeSpeed;
        menuItem.GetComponent<Animator>().Play("FadeIn");
    }

    IEnumerator DisableMenuItem(GameObject menuItem)
    {
        menuItem.GetComponent<Animator>().speed = fadeSpeed;
        menuItem.GetComponent<Animator>().Play("FadeOut");

        yield return new WaitForSecondsRealtime(fadeDuration); // Wait for the fade to actually finish before disabling the object.

        menuItem.SetActive(false);
        if(menuItem == backgroundImage) menuActive = false;

    }
    
    protected virtual void Start()
    {
        fadeSpeed = 1 / fadeDuration;
        idleCountDown = menuIdleTimeout;
        if (backButton != null) backButton.onClick.AddListener(delegate { Back(); });
        if (backButton == null && parentMenu != null) Debug.LogWarning(name + ": You have a parent Menu assigned but no back button.");
    }

    protected virtual void Update()
    {
        if (menuActive && !deactivating && menuTimesOut)
        {
            idleCountDown -= Time.deltaTime;                // Start counting down the idle timer. 

            if (Input.anyKeyDown) idleCountDown = menuIdleTimeout;      // Reset the countdown on any press.

            if (idleCountDown <= 0)
            {
                HideBackgroundImage();
                HideMenuPanel();
                StartCoroutine("Deactivating");
            }
        }
    }

    IEnumerator Deactivating()
    {

        deactivating = true;

        yield return new WaitForSecondsRealtime(fadeDuration);

        deactivating = false;
        idleCountDown = menuIdleTimeout;

    }

    protected virtual void Back()
    {
        // Hide menu
        HideMenuPanel();
        HideBackgroundImage();
        // Show Previous Menu
        if (backButton != null && parentMenu == null)
            Debug.LogError(name + ": Back Button is assigned but the Parent Menu is not");
        else if (parentMenu != null)
        {
            if (Time.timeScale == 0)            
                StartCoroutine("BackCoroutine");            
            else
                parentMenu.GetComponent<MenuController>().Invoke("ShowMenuPanel", fadeDuration);
        }
    }

    private IEnumerator BackCoroutine()
    {
        yield return new WaitForSecondsRealtime(fadeDuration);
        parentMenu.GetComponent<MenuController>().ShowMenuPanel();
    }
}
