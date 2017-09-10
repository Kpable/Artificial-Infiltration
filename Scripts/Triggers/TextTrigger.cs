using System.Collections;
using UnityEngine;

public class TextTrigger : MonoBehaviour {
    public GameObject textCanvasElement;

    [Tooltip("The duration of all fade animations")]
    public float fadeDuration = 0.5f;
    [Tooltip("The time in seconds to wait before disabling text")]
    public float timeout = 5f;
   
    private float fadeSpeed;                // The speed of the animation, always 1/ fadeDuration.
    private bool showing = false;

    private void Start()
    {
        fadeSpeed = 1 / fadeDuration;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if(!showing) StartCoroutine("ShowText");
        }
    }

    IEnumerator ShowText()
    {
        showing = true;
        textCanvasElement.SetActive(true);
        textCanvasElement.GetComponent<Animator>().speed = fadeSpeed;
        textCanvasElement.GetComponent<Animator>().Play("FadeIn");

        yield return new WaitForSecondsRealtime(timeout); // Wait for the fade to actually finish before disabling the object.

        textCanvasElement.GetComponent<Animator>().Play("FadeOut");

        yield return new WaitForSecondsRealtime(fadeDuration); // Wait for the fade to actually finish before disabling the object.

        textCanvasElement.SetActive(false);
        showing = false;

        gameObject.SetActive(false);
    }
}
