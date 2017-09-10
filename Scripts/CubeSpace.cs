using UnityEngine;
using System.Collections;


//How CubeSize works:
// - You can position it wherever. Origin is ideal place for visualization
// - This doesnt care how high you make this so the y axis in scale does not matter
// - X and/or Z axis has to be set, to the same thing preferably but you can set 1 and leave the other alone

public class CubeSpace : MonoBehaviour {


    [Tooltip("Sets the size of the cube for use by other objects")]
    public float cubeSize = 1;                  // The size of the cube
    [HideInInspector]
    public Vector3 origin = Vector3.zero;       // The cube's origin.

	void Awake () {
        if (gameObject.CompareTag("Untagged"))
        {
            Debug.LogWarning(name + ": Cannot function normally without the Cube Tag, I'll add it for you... this time");
            gameObject.tag = "Cube";
        }

        if (transform.localScale.x != 1) cubeSize = transform.localScale.x;
        else if (transform.localScale.z != 1) cubeSize = transform.localScale.z;

        if (cubeSize == 1)
        {
            Debug.LogError(name + ": Cannot function normally without cube size, sorry");
        }

        origin = transform.position;
        Debug.Log("Origin: " + origin.ToString() + " Size " + cubeSize.ToString());
	}

    //This is just for the editor to visualize the cube's size. 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3F);
        // Added an offset to make the wire cube origin be at the bottom of the cube
        Gizmos.DrawWireCube(transform.position + new Vector3(0, transform.localScale.y / 2, 0), transform.localScale );
 
    }

}
