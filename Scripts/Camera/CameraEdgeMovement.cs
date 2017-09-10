using UnityEngine;
using System.Collections;

// This class will assume the X axis is horizontal with the positive end going to the right
// and the Z axis will be vertical with the positive going up.
public class CameraEdgeMovement : MonoBehaviour {

    [HideInInspector]   // not current used
    public float rotationTime = 1.5f;       // Total rotation duration
    [Tooltip("Sets the duration of the movement in seconds")]
    public float movementTime = 0.3f;       // Total movement duration

    //private float currentRotationTime;      // Current amount of time passed since rotation start
    private float currentMovementTime;      // Current amount of time passed since movement start
    private float currentCubeMovementTime;  // Current amount of time passed since cube movement start

    //private bool rotating = false;          // Whether the camera is rotating
    private bool movingAroundCube = false;  // Whether the camera is moving around the cube
    private bool movingToCube = false;      // Whether the camera is moving to a cube

    private float currentAngle = 0;         // The camera's current angle relative to a circle on the X-Z plane
    //private float destinationAngle = 0;     // Where the camera is going on the circle

    private Vector3 destinationPosition;    // The point where the player should be while rotating.
    
    private float radius;                   // Distance of the camera from the world origin Vector3.zero

    private EdgeOfCube currentEdge;         // The current Edge of the cube;
    private EdgeOfCube destinationEdge;     // The destination edge of the cube;

    private CubeSpace nearestCube;

    // Use this for initialization
    void Start () {


        currentEdge = CubeEdges.DetectEdge(transform);

        nearestCube = CubeEdges.DetectNearestCube(transform);

        //Debug.Log(name + "Current Edge: " + currentEdge);


        // The radius is whatever distance away the camera is currently.
        radius = Vector3.Distance(nearestCube.origin, transform.position);

        // Creating a reference point at 0 degrees at distance radius allows for tracking
        // of the camera's position along the circumference of the circle. 
        GameObject refPoint = new GameObject("Rotating Reference Point");
        refPoint.transform.position = new Vector3(radius, 0, 0);

        // Vector3.Angle uses the world origin to tell the angle between two points
        currentAngle = Vector3.Angle(refPoint.transform.position, transform.position);
        // Vector3.Angle only gives us the non reflex angle, ie. always 0 to 180 in either direction
        // So we fix that by adding 180 based on whether the camera is above or below the X axis
        currentAngle += ((transform.position.z >= 0) ? 0 : 180);

        //Debug.Log("Radius: " + radius + " Current angle: " + currentAngle);


    }

    // Update is called once per frame
    void Update () {

        if (movingAroundCube)
        {
            currentMovementTime += Time.deltaTime;      // Add time in seconds
            currentMovementTime = Mathf.Clamp(currentMovementTime, 0, movementTime);    // Dont go more than the set rotationTime

            // Lerp position 0 to 1 for duration movement. 
            float moveLerpPos = currentMovementTime / movementTime;     // Provides position of lerp completion
            if (moveLerpPos >= 1f && movingAroundCube)
            {
                moveLerpPos = 1;                            // Keep it at a max of 1 so we dont accidently go over our desired angle.
                currentEdge = destinationEdge;          // Set the current edge to the destination since we've reached it
                movingAroundCube = false;                       // We're done rotating.
            }

            // Angle between currentAngle and destinationAngle at lerpPos
            float angle = Mathf.LerpAngle(currentEdge.EdgeToAngle(), destinationEdge.EdgeToAngle(), moveLerpPos);

            // Parametric Equation for rotating around a circle
            Vector3 pos = transform.position;
            pos.x = nearestCube.origin.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            pos.z = nearestCube.origin.z + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            transform.position = pos;

        }

        if (movingToCube)
        {
            currentCubeMovementTime += Time.deltaTime;                                      // Add time in seconds
            currentCubeMovementTime = Mathf.Clamp(currentCubeMovementTime, 0, movementTime);    // Dont go more than the set rotationTime

            // Lerp position 0 to 1 for duration rotationTime. 
            float moveToCubeLerpPos = currentCubeMovementTime / movementTime;
            currentMovementTime = Mathf.Clamp(currentCubeMovementTime, 0, movementTime);

            if (moveToCubeLerpPos >= 1 && movingToCube)
            {
                moveToCubeLerpPos = 1;                            // Keep it at a max of 1 so we dont accidently go over our desired angle.
                transform.position = destinationPosition;          // Set the current edge to the destination since we've reached it
                movingToCube = false;                       // We're done rotating.
            }

            // Angle between currentAngle and destinationAngle at lerpPos
            Vector3 pos = Vector3.Lerp(transform.position, destinationPosition, moveToCubeLerpPos);
            transform.position = pos;

        }
    }

    public void RotateAroundCorner(EdgeOfCube cornerEdge)
    {
        //Debug.Log(name + " Rotating around corner");

        currentMovementTime = 0;        // Reset Movement time

        //// Add or subtract 90 degrees to move along the circle

        destinationEdge = CubeEdges.AngleToEdge(
            currentEdge.EdgeToAngle() + (RotateClockwise(cornerEdge) ? -90f : 90f));

        //Debug.Log(name + " Current Angle: " + currentEdge.EdgeToRotation() + " to Destination Angle: " + destinationEdge.EdgeToRotation());

        movingAroundCube = true;          // Start Moving!

    }

    bool RotateClockwise(EdgeOfCube corner)
    {
        // Lets figure out which direction we are turning!

        bool clockwise = false;

        // The four ways we can turn clockwise
        if (currentEdge == EdgeOfCube.Bottom && corner == EdgeOfCube.BottomLeftCorner ||
            currentEdge == EdgeOfCube.Left && corner == EdgeOfCube.TopLeftCorner ||
            currentEdge == EdgeOfCube.Top && corner == EdgeOfCube.TopRightCorner ||
            currentEdge == EdgeOfCube.Right && corner == EdgeOfCube.BottomRightCorner)
        {
            clockwise = true;
        }

        return clockwise;
    }

    // Rotate to a specific edge 
    public void RotateToEdge(EdgeOfCube edge)
    { 
        currentMovementTime = 0;        // Reset Movement time
        destinationEdge = edge;         // Set our goal
        movingAroundCube = true;        // Begin moving        
    }

    public void MoveToPosition(float angle)
    {
        // Parametric Equation for rotating around a circle
        Vector3 pos = transform.position;
        pos.x = nearestCube.origin.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        pos.z = nearestCube.origin.z + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        destinationPosition = pos;

        currentCubeMovementTime = 0;        // Reset the timer

        movingToCube = true;
    }

    public void MoveAndRotateToEdgePosition(CubeSpace desCube, EdgeOfCube edge)
    {
        if (nearestCube != desCube)             // If we are changing cubes
        {
            nearestCube = desCube;              // Set the new cube
            radius = desCube.cubeSize;          // Change the radius
            MoveToPosition(edge.EdgeToAngle());
        }

        if (currentEdge != edge)
        {
            RotateToEdge(edge);
        }
    }
}
