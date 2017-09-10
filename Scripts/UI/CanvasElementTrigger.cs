using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CanvasElementTrigger : MonoBehaviour {

    public GameObject canvasElement;
    
    [Tooltip("The duration of all fade animations")]
    public float fadeDuration = 0.5f;
    [Tooltip("The time in seconds to wait before disabling menus")]
    public float fadeTimeout = 5f;

    float fadeSpeed;                // The speed of the animation, always 1/ fadeDuration.

    // Use this for initialization
    void Start()
    {
        fadeSpeed = 1 / fadeDuration;
        if (canvasElement)
        {
            if (canvasElement.GetComponent<Animator>() == null)
                Debug.LogError(name + ": Canvas Element '" + canvasElement.name + "' does not have an Animator to fade it.");
            if (canvasElement.GetComponent<CanvasGroup>() == null)
            {
                Debug.LogError(name + ": Canvas Element '" + canvasElement.name + "' does not have an Canvas Group adding it.");
                canvasElement.AddComponent<CanvasGroup>();

            }

        }
        else Debug.LogError(name + ": No canvas element attached.");
    }


    IEnumerator ShowCanvasElement()
    {
        canvasElement.SetActive(true);
        canvasElement.GetComponent<Animator>().speed = fadeSpeed;
        canvasElement.GetComponent<Animator>().Play("FadeIn");

        yield return new WaitForSecondsRealtime(fadeTimeout);

        canvasElement.GetComponent<Animator>().speed = fadeSpeed;
        canvasElement.GetComponent<Animator>().Play("FadeOut");
        gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(fadeDuration);
        canvasElement.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        // If we have a canvas element and it is currently not active, fade it in. 
        if(canvasElement && !canvasElement.activeInHierarchy) StartCoroutine("ShowCanvasElement");
    }

}
