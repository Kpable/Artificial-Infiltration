using UnityEngine;
using System.Collections;

public class WASD_Movement : MonoBehaviour {

    [Tooltip("Sets the maximum speed for the player")]
    public float speed = 1;         //How fast the player can move horizontally  

    private bool onWallLeft;        //Checks if the player has hit a wall
    private bool onWallRight;       //Checks if the player has hit a wall

    private Rigidbody rigidBody;    //Reference to the rigid body component on parent object

    //private GameObject cameraObject;      //Camera object in scene

    /// /////////////////////////////////////////////////////////////////////////////////////
    /// Runs code on start of scene
    void Start()
    {
        onWallLeft = false;                         //Start off not on the wall
        onWallRight = false;                        //Start off not on the wall

        rigidBody = GetComponent<Rigidbody>();      //Component reference for RigidBody
        //cameraObject = GameObject.Find("Main Camera");    //Object Reference for camera in scene
    }

    /// /////////////////////////////////////////////////////////////////////////////////////
    /// Update is called once per frame
    void Update()
    {
        ////////Move right when pressing "D" key
        if (Input.GetKeyDown(KeyCode.D))                                    //Stops horizontal movement when button initially pressed
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.D))                                        //Moves the player every frame the key is held down
        {
            if (rigidBody.velocity.y < 0)                                   //Makes sure player is always falling 
            {
                rigidBody.AddForce(transform.up * -1);
            }

            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);   //Stops all horizontal movement to prevent momentum stacking 

            if (onWallLeft == false)                                        //Move the player if they're not touching a wall
            {
                rigidBody.AddForce(transform.right * speed);
            }
        }

        ////////Move left when pressing "A" key
        if (Input.GetKeyDown(KeyCode.A))                                    //Stops horizontal movement when button is initiall pressed
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.A))                                        //Moves the player every frame the key is held down
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);   //Stops all horizontal movement to prevent momentum stacking 

            if (onWallRight == false)                                       //Moves the player if they're not touching a wall
            {
                rigidBody.AddForce(transform.right * speed * -1);
            }
            if (rigidBody.velocity.y < 0)                                   //Makes sure the player is always falling
            {
                rigidBody.AddForce(transform.up * -1);
            }
        }
    }

    /// /////////////////////////////////////////////////////////////////////////////////////
    /// Check for collisions when entering a trigger
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("WallSideLeft"))      //Detects when hitting a left wall
        {
            onWallLeft = true;
        }
        if (col.gameObject.CompareTag("WallSideRight"))     //Detects when hitting a right wall
        {
            onWallRight = true;
        }
    }
    /// /////////////////////////////////////////////////////////////////////////////////////
    /// Checks for leaving a trigger 
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("WallSideLeft"))    //Detects when hitting a left wall
        {
            onWallLeft = false;
        }
        if (col.gameObject.CompareTag("WallSideRight"))   //Detects when hitting a right wall
        {
            onWallRight = false;
        }
    }
}