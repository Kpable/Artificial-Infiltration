using UnityEngine;
using System.Collections;


//TODO Custom Sizing seems pointless now that EdgeOfCube has position information. 
// If pointless remove. 
public class Corners : MonoBehaviour {

    [Tooltip("The corner trigger to use for all the corners")]
    public GameObject cornerTrigger;
    [Tooltip("Enables you to change the size of all four cubes in game")]
    public bool editInGame = false;         // Lets the designer play with the size and position.
    [Tooltip("Sets the size of the cube for this mission")]
    public float customSize = 10f;

    private float size;

    private GameObject[] corners;
    
    private CubeSpace nearestCube;

    // Use this for initialization
    void Start () {
        nearestCube = CubeEdges.DetectNearestCube(transform);
        // Grab the size, half for pos from origin
        size = nearestCube.cubeSize / 2;

        corners = new GameObject[4];
	    if(cornerTrigger != null)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject corner = Instantiate(cornerTrigger);
                corner.transform.parent = transform;
                corners[i] = corner;
            }
            
        }
        PositionCornerTriggers();
	}
	
    // Sets a corner at the 4 corners
    void PositionCornerTriggers()
    {
        if (editInGame) size = customSize;
        else size = nearestCube.cubeSize / 2;

        //TODO this has to be .5 somehow. either the size or the size/2. Why?
        size -= corners[0].GetComponent<BoxCollider>().size.x / 2;

        Vector3 pos = new Vector3(size + nearestCube.origin.x, 0, size + nearestCube.origin.z);
        corners[0].transform.position = pos;
        corners[0].GetComponent<Corner_Switch>().SetCorner(EdgeOfCube.TopRightCorner);

        pos = new Vector3(-size + nearestCube.origin.x, 0, size + nearestCube.origin.z);
        corners[1].transform.position = pos;
        corners[1].GetComponent<Corner_Switch>().SetCorner(EdgeOfCube.TopLeftCorner);

        pos = new Vector3(size + nearestCube.origin.x, 0, -size + nearestCube.origin.z);
        corners[2].transform.position = pos;
        corners[2].GetComponent<Corner_Switch>().SetCorner(EdgeOfCube.BottomRightCorner);

        pos = new Vector3(-size + nearestCube.origin.x, 0, -size + nearestCube.origin.z);
        corners[3].transform.position = pos;
        corners[3].GetComponent<Corner_Switch>().SetCorner(EdgeOfCube.BottomLeftCorner);
    }

    // Update is called once per frame
    void Update () {
        if (editInGame)         // disabled by default to avoid unnecessary calls to readjust position
        {
            PositionCornerTriggers();
        }
	}
}
