using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour
{

    public float speed = 1;         //How fast the player can move horizontally  
    public float jump = 1;			//How fast the player moves upwards when jumping 
    public float falling = 25;      //Vertical force downwards while falling.  

    private bool onWallLeft;
    private bool onWallRight;

    private Rigidbody rigidBody;    //Reference to the rigid body component on parent object
    private bool onGround;          //Boolean for if player is on the ground

    //private GameObject camera;      //Camera object in scene

    /// /////////////////////////////////////////////////////////////////////////////////////
    /// Runs code on start of scene
    void Start()
    {

        onGround = false;                           //Start with jumping off (prevents bugs)
        onWallLeft = false;                         //Start off not on the wall
        onWallRight = false;

        rigidBody = GetComponent<Rigidbody>();      //Component reference for RigidBody
       // camera = GameObject.Find("Main Camera");    //Object Reference for camera in scene
    }

    /// /////////////////////////////////////////////////////////////////////////////////////
    /// Update is called once per frame
    void Update()
    {
        ////////Move right when pressing "D" key
        if (Input.GetKeyDown(KeyCode.D))
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (rigidBody.velocity.y < 0)
            {
                rigidBody.AddForce(transform.up * -1);
            }

            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
            if (onWallLeft == false)
            {
                rigidBody.AddForce(transform.right * speed);
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        }

        ////////Move left when pressing "A" key
        if (Input.GetKeyDown(KeyCode.A))
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
            if (onWallRight == false)
            {
                rigidBody.AddForce(transform.right * speed * -1);
            }
            if (rigidBody.velocity.y < 0)
            {
                rigidBody.AddForce(transform.up * falling * -1);
            }
        }

        ////////Jump when pressing "W" key
        if (Input.GetKey(KeyCode.W))
        {

            if (onGround)
            {
                rigidBody.AddForce(transform.up * jump);
                onGround = false;
            }
        }
    }

    /// /////////////////////////////////////////////////////////////////////////////////////
    /// Check for collisions that occur by the player in scene
    void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit Ground");
            onGround = true;
        }
        if (collide.gameObject.CompareTag("WallSideLeft"))
        {
            Debug.Log("Hit Left");
            onWallLeft = true;
        }
        if (collide.gameObject.CompareTag("WallSideRight"))
        {
            Debug.Log("Hit Right");
            onWallRight = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
       if (collision.gameObject.CompareTag("WallSideLeft"))
        {
            onWallLeft = false;
        }
       if (collision.gameObject.CompareTag("WallSideRight"))
        {
            onWallRight = false;
        }
    }
}