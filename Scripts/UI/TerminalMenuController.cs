using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TerminalMenuController : MenuController {
    private AudioSource audioSource;
    private string terminalText;

    private Text terminalScreen;

    private string terminalOutput;
    private string[] terminalLines;
    private int printLine;
    public float lineScrollSpeed = 0.025f;
    private float nextLineWait = 0;

    void Awake()
    {
        /*
        // Get or add the audio for the sound effect.
        audioSource = GetComponentInChildren<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning(name + " Cannot find audio source to play sound effects. Adding it.");
            transform.FindChild("SFX").gameObject.AddComponent<AudioSource>();
            audioSource = GetComponentInChildren<AudioSource>();
        }
        */
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();

        terminalScreen = this.transform.Find("Terminal Panel").transform.Find("Terminal").gameObject.GetComponent<Text>();//this.gameObject.GetComponent<Text>();
        terminalText = terminalScreen.text;
        terminalLines = terminalText.Split(new string[] { "\n" }, StringSplitOptions.None);

        Debug.Log("TEST " + terminalLines.Length);

        terminalScreen.text = terminalLines[0];

        StartCoroutine(updateScreen());
    }



    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //StartCoroutine("PauseGame");
        }
    }

    IEnumerator updateScreen()
    {
        yield return new WaitForSecondsRealtime(lineScrollSpeed + nextLineWait);
        nextLineWait = 0;

        if (printLine > terminalLines.Length)
        {
            yield break;
        }

        string curLine = terminalLines[printLine];

        if (curLine.Contains("++fadeIn"))
        {
            //ambientMusic.SetActive(true);
            //gameMusic.SetActive(true);
            Utils.StartTime();

            HideBackgroundImage();
            HideMenuPanel();

            yield break;
        }

        // Check if the text UI component is full (Todo: replace constant integer padding with something more dynamic)
        if (terminalScreen.preferredHeight >= (this.gameObject.GetComponent<RectTransform>().rect.height))
        {
            // Delete the first line in the output array
            terminalOutput = terminalOutput.Substring(terminalOutput.IndexOf(System.Environment.NewLine) + System.Environment.NewLine.Length);
        }

        // Check for type markups
        if (curLine.Contains("++type") || curLine.Contains("++slowtype"))
        {
            // Slow type or normal
            string typeMarkup = "";
            if (curLine.Contains("++type"))
            {
                typeMarkup = "++type";
            }
            else if (curLine.Contains("++slowtype"))
            {
                typeMarkup = "++slowtype";
            }

            string[] lineElements = curLine.Split(new[] { typeMarkup }, StringSplitOptions.None);
            lineElements[1] = lineElements[1].Replace(typeMarkup, String.Empty);
            curLine = curLine.Replace(typeMarkup, String.Empty);

            // Print before the typed string
            terminalScreen.text += lineElements[0];

            // Wait for a bit for realism
            yield return new WaitForSecondsRealtime(1);

            // Handle any end waits (to simulate a carriage return)
            // Check for markup (because actually animating things is for plebs)
            if (lineElements[1].Contains("++wait"))
            {
                // Get the actual wait value
                nextLineWait = (float)Char.GetNumericValue(lineElements[1][lineElements[1].IndexOf("++wait") + 6]);
                lineElements[1] = lineElements[1].Replace("++wait" + lineElements[1][lineElements[1].IndexOf("++wait") + 6], String.Empty);
            }

            // "Type" the rest of the line
            foreach (char character in lineElements[1])
            {
                terminalScreen.text += character;

                if (typeMarkup == "++type")
                {
                    yield return new WaitForSecondsRealtime(0.05f);
                }
                else if (typeMarkup == "++slowtype")
                {
                    yield return new WaitForSecondsRealtime(0.15f);
                }
            }

        }

        // Check for markup (because actually animating things is for plebs)
        if (curLine.Contains("++wait"))
        {
            // Get the actual wait value
            nextLineWait = (float)Char.GetNumericValue(curLine[curLine.IndexOf("++wait") + 6]);
            curLine = curLine.Replace("++wait" + curLine[curLine.IndexOf("++wait") + 6], String.Empty);
        }

        terminalOutput += curLine + "\n";
        printLine++;

        if (curLine.Contains("++clear"))
        {
            terminalOutput = "";
        }

        if (!curLine.Contains("++type") && !curLine.Contains("++slowtype"))
        {
            terminalScreen.text = terminalOutput;
        }

        if (printLine < terminalLines.Length)
        {
            StartCoroutine(updateScreen());
        }
    }

}