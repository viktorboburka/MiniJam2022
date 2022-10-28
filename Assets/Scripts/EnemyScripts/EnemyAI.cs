using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    private GameObject player;
    private NavMeshAgent navAgent;
    private Enemy enemy;

    private float lastAttackTime = -Mathf.Infinity;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navAgent = this.GetComponent<NavMeshAgent>();
        enemy = gameObject.GetComponent<Enemy>();
    }

    void Update()
    {
        if (GetDistanceFromPlayer() >= enemy.getPursueDistance()) {
            navAgent.SetDestination(player.transform.position);
        }
        if (PlayerInRange() && ReadyToAttack()) {
            Attack();
        }
    }

    private float GetDistanceFromPlayer() {
        return Vector3.Distance(this.transform.position, player.transform.position);
    }

    private bool PlayerInRange() {
        if (GetDistanceFromPlayer() <= enemy.getRange()) {
            return true;
        }
        return false;
    }

    private bool ReadyToAttack() {
        if (Time.time > lastAttackTime + 1.0 / enemy.getAttackSpeed()) {
            return true;
        }
        return false;
    }

    private void Attack() {
        Debug.Log(this + " attacked player at " + Time.time);
        lastAttackTime = Time.time;
        
    }
}
