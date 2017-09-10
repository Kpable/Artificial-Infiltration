using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private GameObject player;
    NavMeshAgent agent;

    void Start () {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player");
	}

    void Update () {
        agent.destination = player.transform.position;
	}

    // Detects if player has been hit
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Player Died!");
        }
    }
}
