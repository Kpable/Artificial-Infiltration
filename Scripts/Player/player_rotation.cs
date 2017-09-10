using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_rotation : MonoBehaviour {

            [Tooltip("The Speed of the rotation")]
    public float rotationSpeed = 50f;

    void FixedUpdate()
    {
        transform.Rotate(transform.up, rotationSpeed * Time.fixedDeltaTime, Space.Self);
    }

}   
