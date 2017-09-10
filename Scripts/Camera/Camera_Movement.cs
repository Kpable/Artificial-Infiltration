using UnityEngine;
using System.Collections;

public class Camera_Movement : MonoBehaviour {


    public bool lookAtPlayer = false;

	//private int speed; //movement speed value
    
	private GameObject player;      // Player object for reference
	private GameObject focusPoint;  // Focus point object for reference

    
    //////////////////////////////////////////////////////////////////////////////////
    // Use this for initialization
    void Start () {
		//Grab a local reference of object in scene
		player = GameObject.Find ("Player");
		focusPoint = GameObject.Find ("Camera Focus Point");

        Debug.Log("Camera size " + Camera.main.pixelWidth + ", " + Camera.main.pixelHeight);
        Debug.Log("Ratio " + (float)Camera.main.pixelWidth / Camera.main.pixelHeight);
        Debug.Log("Normal Ratio " + (16f / 9f));
    }
		
//////////////////////////////////////////////////////////////////////////////////
	//LateUpdate runs as the last thing to be done after all other update functions
	void LateUpdate () {
        if(lookAtPlayer)
            transform.LookAt(player.transform.position);
        else 
            transform.LookAt(focusPoint.transform.position);

    }
    //////////////////////////////////////////////////////////////////////////////////
}

