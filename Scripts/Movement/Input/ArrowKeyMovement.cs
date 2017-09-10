using UnityEngine;
using System.Collections;

public class ArrowKeyMovement : MonoBehaviour {

    [Tooltip("Sets the maximum speed for the player")]
    public float maxSpeed = 10;         //How fast the player can move horizontally  
    [Tooltip("Sets the movement force for the player")]
    public float moveForce = 1;         //How fast the player can move horizontally  
    [Tooltip("Sets the percentage of normal speed the player can move while in the air")]
    public float midAirHorizontalSpeedModifier = 0.3f;         //How fast the player can move horizontally  
 
    // Whether to stop movement on button release
    public bool stopGroundHorizontalMovement = false;          
    public bool stopMidAirHorizontalMovement = false;

    //Checks if the player has hit a wall and what side of the wall it hit. 
    private bool onWallLeft;        
    private bool onWallRight;       
    private bool onWallFront;      
    private bool onWallBack;      
    //private bool onWall;            

    private Rigidbody body;                 //Reference to the rigid body component on parent object

    private float inAirHorizontalSpeed;     // Mid air Speed adjustment. 

    private float hAxis = 0;

    /// /////////////////////////////////////////////////////////////////////////////////////
    /// Runs code on start of scene
    void Start()
    {
        onWallLeft = false;                         //Start off not on the wall
        onWallRight = false;                        //Start off not on the wall

        body = GetComponent<Rigidbody>();      //Component reference for RigidBody
        inAirHorizontalSpeed = moveForce * midAirHorizontalSpeedModifier;

    }

    /// /////////////////////////////////////////////////////////////////////////////////////
    /// Update is called once per frame
    private void Update()
    {
        // Want to collect input in Update to make sure we dont miss a keypress
        hAxis = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {  
        // Want to move rigidBodys in FixedUpdate to keep movement consistant. 
        Move(hAxis);
    }

    private void Move(float hAxis)
    {
        // If in the air and pushing against a wall dont do anything
        if (!IsGrounded() && ( ( (onWallLeft || onWallBack) && hAxis > 0) || ((onWallRight || onWallFront) && hAxis < 0) ) )
            return;

        // Standard force will be moving to the right * set force  * horizontal input (-1, 0, or 1)
        Vector3 force = transform.right * moveForce * hAxis;

        //If in the air, lets modify that force a bit
        if (!IsGrounded())
            force = transform.right * inAirHorizontalSpeed * hAxis;

        // Lets see what edge we're on, based on rotation because of the change around corners. 
        EdgeOfCube currentEdge = CubeEdges.RotationToEdge(transform.rotation.eulerAngles.y);

        // Check if our current speed is less than the max speed. 
        if (hAxis * currentEdge.LeftRightAxis(body.velocity) < maxSpeed )
            body.AddForce(force);

        // If our velocity is over max speed, lets set it to max speed. 
        if (Mathf.Abs(currentEdge.LeftRightAxis(body.velocity) ) > maxSpeed)
        {
            // Making sure we adjust the right axis velocity. 
            if(currentEdge == EdgeOfCube.Top || currentEdge == EdgeOfCube.Bottom)
            {
                body.velocity = new Vector3(Mathf.Sign(body.velocity.x) * maxSpeed, body.velocity.y, body.velocity.z);
            }
            else if(currentEdge == EdgeOfCube.Left || currentEdge == EdgeOfCube.Right)
            {
                body.velocity = new Vector3(body.velocity.x, body.velocity.y, Mathf.Sign(body.velocity.z) * maxSpeed);
            }
        }

        // Finally if we want to stop any movement on release do so. 
        if (stopGroundHorizontalMovement && IsGrounded() && hAxis == 0 || 
            stopMidAirHorizontalMovement && !IsGrounded() && hAxis == 0 )
            body.velocity = new Vector3(0, body.velocity.y, 0);

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
        if (col.gameObject.CompareTag("WallSideFront"))      //Detects when hitting a front wall
        {
            onWallFront = true;
        }
        if (col.gameObject.CompareTag("WallSideBack"))     //Detects when hitting a wall wall
        {
            onWallBack = true;
        }

        //onWall = onWallLeft || onWallRight || onWallFront || onWallBack;

        //if (onWall)        
        //    Debug.Log("Trigger Enter: OnWallLeft: " + onWallLeft + " OnWallRight: " + onWallRight + " OnWallFront " + onWallFront + " OnWallBack " + onWallBack);
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
        if (col.gameObject.CompareTag("WallSideFront"))      //Detects when hitting a front wall
        {
            onWallFront = false;
        }
        if (col.gameObject.CompareTag("WallSideBack"))     //Detects when hitting a back wall
        {
            onWallBack = false;
        }

        //onWall = onWallLeft || onWallRight || onWallFront || onWallBack;
        //if (onWall) 
        //    Debug.Log("Trigger Exit: OnWallLeft: " + onWallLeft + " OnWallRight: " + onWallRight + " OnWallFront " + onWallFront + " OnWallBack " + onWallBack);

    }

    public bool IsGrounded()
    {
        float rayLength = GetComponent<Collider>().bounds.extents.y + 0.1f;
        //Debug.DrawRay(transform.position, -Vector3.up * rayLength);
        return Physics.Raycast(transform.position, -Vector3.up, rayLength);
    }
}