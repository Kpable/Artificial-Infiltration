using UnityEngine;
using System.Collections;

public class PlayerEdgeMovement : MonoBehaviour {


    [Tooltip("Sets the duration of the rotation in seconds")]
    public float rotationTime = 1.5f;       // Total rotation duration

    public float movementTime = 0.3f;       // Total movement duration

    public float amountPushedFromCorner = 1f;

    private float currentRotationTime;      // Current amount of time passed since rotation start
    private float currentMovementTime;      // Current amount of time passed since movement start

    private bool rotating = false;          // Whether the player is rotating
    private bool moving = false;            // Whether the player is moving

    private Vector3 playerVelocity;         // The velocity of the player when the trigger is entered. 

    private Vector3 destinationPosition;    // The point where the player should be while rotating.

    private EdgeOfCube currentEdge;         // The current Edge of the cube;
    private EdgeOfCube destinationEdge;     // The destination edge of the cube;

    // Use this for initialization
    void Start () {

        //TODO Force current angle to the right edge. Might be causing infinite switching. 
        // The player's angle is based on its y rotation. 
        //currentAngle = transform.rotation.eulerAngles.y;

        currentEdge = CubeEdges.DetectEdge(transform);
    }
	
	// Update is called once per frame
	void Update () {
        if (rotating)
        {
            currentRotationTime += Time.deltaTime;                                      // Add time in seconds
            currentRotationTime = Mathf.Clamp(currentRotationTime, 0, rotationTime);    // Dont go more than the set rotationTime

            // Lerp position 0 to 1 for duration rotationTime. 
            float lerpPos = currentRotationTime / rotationTime;
            currentRotationTime = Mathf.Clamp(currentRotationTime, 0, rotationTime);

            if (lerpPos >= 1 && rotating)
            {               
                lerpPos = 1;                            // Keep it at a max of 1 so we dont accidently go over our desired angle.
                //currentAngle = destinationAngle;        // Set the current angle to the destination since we've reached it
                currentEdge = destinationEdge;          // Set the current edge to the destination since we've reached it
                rotating = false;                       // We're done rotating.

                // need to make sure you all animation is complete before enabling movement again or else ISSUES! with corners
                if (!rotating && !moving)
                    EnablePlayerMovement(true);

            }

            // Angle between currentAngle and destinationAngle at lerpPos
            //float angle = Mathf.LerpAngle(currentAngle, destinationAngle, lerpPos);
            float angle = Mathf.LerpAngle(currentEdge.EdgeToRotation(), destinationEdge.EdgeToRotation(), lerpPos);

            // Grab and set the player's rotation.
            Vector3 rot = transform.rotation.eulerAngles;
            rot.y = angle;
            transform.rotation = Quaternion.Euler(rot);
        }

        if (moving)
        {

            currentMovementTime += Time.deltaTime;                                      // Add time in seconds
            currentMovementTime = Mathf.Clamp(currentMovementTime, 0, movementTime);    // Dont go more than the set rotationTime

            // Lerp position 0 to 1 for duration rotationTime. 
            float moveLerpPos = currentMovementTime / movementTime;
            currentMovementTime = Mathf.Clamp(currentMovementTime, 0, movementTime);

            if (moveLerpPos >= 1 && moving)
            {
                moveLerpPos = 1;                            // Keep it at a max of 1 so we dont accidently go over our desired angle.
                transform.position = destinationPosition;          // Set the current edge to the destination since we've reached it
                moving = false;                       // We're done rotating.

                // need to make sure you all animation is complete before enabling movement again or else ISSUES! with corners
                if (!rotating && !moving)
                    EnablePlayerMovement(true);

            }

            // Angle between currentAngle and destinationAngle at lerpPos
            Vector3 pos = Vector3.Lerp(transform.position, destinationPosition, moveLerpPos);
            transform.position = pos;
        }
    }

    // This method may be more suitable in the PlayerMovement Class
    private void EnablePlayerMovement(bool enabled)
    {
        gameObject.GetComponent<JumpMovement>().enabled = enabled;
        gameObject.GetComponent<ArrowKeyMovement>().enabled = enabled;
        //gameObject.GetComponent<WASD_Movement>().enabled = enabled;
        gameObject.GetComponent<Rigidbody>().useGravity = enabled;
        //gameObject.GetComponent<ClampToCubeEdges>().enabled = enabled;


        //TODO this doesnt retain velocity around corners.
        //if (!enabled)
        //{
        //    playerVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        //    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}            
        //else
        //    gameObject.GetComponent<Rigidbody>().velocity = playerVelocity;

        // Teleportation disables these
        if (!gameObject.GetComponent<ClampToCubeEdges>().enabled && enabled)
            gameObject.GetComponent<ClampToCubeEdges>().enabled = true;
        if (!gameObject.GetComponent<BoxCollider>().enabled && enabled)
            gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void RotateAroundCorner(EdgeOfCube cornerEdge)
    {
        //Debug.Log(name + " Rotating around corner");

        destinationPosition = cornerEdge.Pos(transform);

        ////Debug.Log("Destination Position: " + destinationPosition.ToString());

        currentRotationTime = 0;        // Reset rotation time
        currentMovementTime = 0;        // Reset movement time

        // Add or subtract 90 degrees to move along the circle
        bool clockwiseRotation = RotateClockwise(cornerEdge);

        // Although normal clockwise rotation is -90 about a circle, rotation clockwise about 
        // one's self is +90 degrees. Thats why the numbers are inverted.
        destinationEdge = CubeEdges.RotationToEdge(
            currentEdge.EdgeToRotation() + (clockwiseRotation ? 90f : -90f));

        //Push the player out of corner trigger
        if ((clockwiseRotation && destinationEdge == EdgeOfCube.Left) ||
            (!clockwiseRotation && destinationEdge == EdgeOfCube.Right))
        {
            destinationPosition.z += transform.localScale.x + amountPushedFromCorner;
        }
        else if ((clockwiseRotation && destinationEdge == EdgeOfCube.Top) ||
            (!clockwiseRotation && destinationEdge == EdgeOfCube.Bottom))
        {
            destinationPosition.x += transform.localScale.x + amountPushedFromCorner;
        }
        else if ((clockwiseRotation && destinationEdge == EdgeOfCube.Right) ||
            (!clockwiseRotation && destinationEdge == EdgeOfCube.Left))
        {
            destinationPosition.z -= transform.localScale.x + amountPushedFromCorner;
        }
        else if ((clockwiseRotation && destinationEdge == EdgeOfCube.Bottom) ||
            (!clockwiseRotation && destinationEdge == EdgeOfCube.Top))
        {
            destinationPosition.x -= transform.localScale.x + amountPushedFromCorner;
        }

        //Debug.Log(name + " Current Angle: " + currentEdge.EdgeToRotation() + " to Destination Angle: " + destinationEdge.EdgeToRotation());

        // Stop the player from moving past the corner or jumping while rotating
        EnablePlayerMovement(false);

        rotating = true;        // Start Rotating!
        moving = true;          // Start Moving!

    }

    bool RotateClockwise(EdgeOfCube corner)
    {
        // Lets figure out which direction we are turning!

        bool clockwise = false;

        // The four ways we can turn clockwise
        if (currentEdge == EdgeOfCube.Bottom && corner == EdgeOfCube.BottomLeftCorner ||
            currentEdge == EdgeOfCube.Left && corner == EdgeOfCube.TopLeftCorner  ||
            currentEdge == EdgeOfCube.Top && corner == EdgeOfCube.TopRightCorner ||
            currentEdge == EdgeOfCube.Right && corner == EdgeOfCube.BottomRightCorner)
        {
            clockwise =  true;
        }

        return clockwise;
    }

    // Move the player to a specific edge
    public void  RotateToEdgeRotation(EdgeOfCube edge)
    {
        // Only rotate to the edge if its different.
        if (edge != currentEdge)
        {
            currentRotationTime = 0;            // Reset rotation time
            destinationEdge = edge;             // Set our goal
            rotating = true;                    // Begin rotating
        }
    }
    
    // Move the player to a specific position
    public void MoveToPosition(Vector3 des)
    {
        currentMovementTime = 0;                // Reset movement time
        destinationPosition = des;              // Set our goal 
        moving = true;                          // Begin Movement
    }

    // Move the player to a specific position and Edge
    public void MoveAndRotateToEdgePosition(Transform destination)
    {
        EdgeOfCube edgeDes = CubeEdges.DetectEdge(destination);

        EnablePlayerMovement(false);
        
        RotateToEdgeRotation(edgeDes);
        MoveToPosition(destination.position);

    }

}
