using UnityEngine;
using System.Collections;

public class Corner_Switch : MonoBehaviour {

	private GameObject player;
	private GameObject cameraObject;

    private bool insideTrigger = false;          // Dont want to re-trigger if already rotating. 

    private EdgeOfCube edge;


	/// /////////////////////////////////////////////////////////////////////////////////////
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		cameraObject = GameObject.Find ("Main Camera");
	}
	/// /////////////////////////////////////////////////////////////////////////////////////

    void OnTriggerEnter(Collider col)
    {
        if (!insideTrigger && col.gameObject.name == "Player")
        {
            //Debug.Log("Collided: " + col.gameObject.name);

            //camera.GetComponent<CameraCornerMovement>().RotateAroundCorner(transform.position);
            //player.GetComponent<PlayerCornerMovement>().RotateAroundCorner(transform.position);

            if (cameraObject)cameraObject.GetComponent<CameraEdgeMovement>().RotateAroundCorner(edge);
            player.GetComponent<PlayerEdgeMovement>().RotateAroundCorner(edge);
            // TODO Clamp player only while in corner
            insideTrigger = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            //Debug.Log("Collided: " + col.gameObject.name);

            insideTrigger = false;
        }
    }

    public void SetCorner(EdgeOfCube corner)
    {
        edge = corner;
    }
}