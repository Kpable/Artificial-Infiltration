using UnityEngine;
using System.Collections;

public class FollowPlayerVerticalPosition : MonoBehaviour {

    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 pos = transform.position;
        pos.y = player.transform.position.y;
        transform.position = pos;
	}
}
