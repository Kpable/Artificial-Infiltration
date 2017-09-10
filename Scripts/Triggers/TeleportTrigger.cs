using UnityEngine;

public class TeleportTrigger : MonoBehaviour {

    public Transform teleportDestination;

    private EdgeOfCube edge;

    void Start()
    {
        //Figure out which edge the destination is on. 
        edge = CubeEdges.DetectEdge(teleportDestination);
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log(name + " was triggered by: " + col.gameObject.name);

        if (col.gameObject.CompareTag("Player"))
        {
            // Move the focus point to new Cube
            GameObject.Find("Camera Focus Point").transform.position = CubeEdges.DetectNearestCube(teleportDestination).origin;

            // Disable player's clamping
            col.gameObject.GetComponent<ClampToCubeEdges>().enabled = false;

            // let the player move through solid objects
            col.gameObject.GetComponent<BoxCollider>().enabled = false;
            
            //Debug.Log("Teleport Trigger: " + name + " at position " + transform.position);
            
            // Rotate the player appropriately and move him to our teleport destination
            col.gameObject.GetComponent<PlayerEdgeMovement>().MoveAndRotateToEdgePosition(teleportDestination);

            // Move and rotate the camera as well. 
            Camera.main.GetComponent<CameraEdgeMovement>().MoveAndRotateToEdgePosition(CubeEdges.DetectNearestCube(teleportDestination), edge);

        }
    }
}
