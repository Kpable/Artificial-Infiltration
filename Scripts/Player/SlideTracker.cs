using UnityEngine;
using System.Collections;

public class SlideTracker : MonoBehaviour {

	private bool buttonReleased;	 //Tracks when the button is not being pressed

	private Vector3 oldPosition;	//Tracks previously recorded position of object
	private Rigidbody rigid;		//Reference for RigidBody on object

	// Use this for initialization
	void Start () {
		oldPosition = transform.position;	//Take the starting player position (prevents bugs)
		rigid = GetComponent<Rigidbody> ();	//Set RigidBody reference
	}
	
	// Update is called once per frame
	void Update () {
		if (buttonReleased) {											//Check if the button is not being pressed
			if (oldPosition != transform.position) {					//Check if the player is in a new position 
				rigid.velocity = new Vector3 (0, rigid.velocity.y, 0);	//Set horizontal movement to ZERO
			}
		}

		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) {     //Check for horizontal movement (Presses)
        //if (Input.GetAxisRaw("Horizontal") != 0 ) {		//Check for horizontal movement (Presses)
			buttonReleased = false;										//Button is being pressed, so false
			oldPosition = transform.position;							//Grab position of button press (tracks last)
		} else {
			buttonReleased = true;										//No key being pressed, so true
		}
	}
}
