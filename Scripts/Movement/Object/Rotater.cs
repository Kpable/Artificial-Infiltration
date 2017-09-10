using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour {

    [Tooltip ("The Speed of the rotation")]
    public float rotationSpeed = 50f;

	void FixedUpdate () {
        transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime, Space.World);
	}
}
