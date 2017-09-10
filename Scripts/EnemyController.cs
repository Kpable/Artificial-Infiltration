using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour {

    private GameObject player;
    NavMeshAgent agent;

    /// /////////////////////////////////////////////////////////////////////////////////////
    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player");
	}
    /// /////////////////////////////////////////////////////////////////////////////////////
    // Update is called once per frame
    void Update () {
        agent.destination = player.transform.position;
	}
    /// /////////////////////////////////////////////////////////////////////////////////////
    /// Detects if player has been hit
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Player Died!");
        }
    }
}
