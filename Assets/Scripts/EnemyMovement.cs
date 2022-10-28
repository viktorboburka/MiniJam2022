using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    private GameObject player;
    private NavMeshAgent navAgent;

    //enemies stop pursuing the player if they get too close
    [SerializeField]
    private float pursueDistance = 0.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navAgent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (GetDistanceFromPlayer() >= pursueDistance) {
            navAgent.SetDestination(player.transform.position);
        }
    }

    private float GetDistanceFromPlayer() {
        return Vector3.Distance(this.transform.position, player.transform.position);
    }
}
