using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour {

    public Transform target;
    public float rotateSpeed = 3f;

    float angle = 0;
    public float radius = 10;

	// Use this for initialization
	void Start () {
		if(target == null)
        {
            Debug.LogError(name + " needs an target to rotate around. Disabling script.");
            gameObject.GetComponent<RotateAroundObject>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        angle += rotateSpeed * Time.deltaTime;
        if (angle > 360) angle -= 360;

        //transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime );
        Vector3 pos = transform.position;
        pos.x = target.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        pos.z = target.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        transform.position = pos;

        transform.LookAt(target);
    }
}
