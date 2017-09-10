using UnityEngine;

public class ClampToCubeEdges : MonoBehaviour {

    public bool continuousClamping = false;         // Whether to continuously clamp the object

    private EdgeOfCube currentEdge;

    void Start()
    {
        currentEdge = CubeEdges.DetectEdge(transform);        // Detect current edge of cube based on transform. 

        if(transform.rotation.eulerAngles.y != currentEdge.EdgeToRotation())    // Fix rotation if necessary
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, currentEdge.EdgeToRotation(), transform.rotation.z));

        //Debug.Log("Current Edge: " + currentEdge);
        Clamp();
    }

    // Update is called once per frame
    void LateUpdate () {
        // Added to reduce the number of calls to detection methods. Theres as whole lotta maths over there. 
        if (continuousClamping)
        {
            Clamp();
        }
    }

    //TODO reduce calls even further by only calling while in corner triggers
    private void Clamp()
    {
        transform.position = CubeEdges.Clamp(transform);
    }
}
