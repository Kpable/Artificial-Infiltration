using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent (typeof (Text))]
public class InGameTimer : MonoBehaviour {

    Text inGameTimerText;                   // UI Text component for showing the time
    DateTime time = DateTime.MinValue;      // The DateTime object to keep time data neat
    bool running = true;                    // Whether the timer is running or not. 

    void Awake()
    {
        inGameTimerText = GetComponent<Text>();
    }

	// Use this for initialization
	void Start () {
        time = time.AddSeconds(Time.timeSinceLevelLoad);        // Add the time since the start of the level
    }
	
	// Update is called once per frame
	void Update () {
        if (running)    // If the timer is running, add time. 
        {
            time = time.AddSeconds( Time.deltaTime );           // Increment the time by each frame's deltaTime
        }
        
        // Make it pretty
        inGameTimerText.text = time.Minute.ToString("00") + ":" + time.Second.ToString("00") + "." + time.Millisecond.ToString("000");
	}

    // Starts timer
    public void StartTimer()
    {
        running = true;
    }

    // Stops Timer
    public void StopTimer()
    {
        running = false;
    }

    public void ResetTimer()
    {
        time = DateTime.MinValue;
    }

    public DateTime GetTime()
    {
        return time;
    }
}
