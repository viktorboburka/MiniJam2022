using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    private GameObject player;
    private NavMeshAgent navAgent;
    private Enemy enemy;

    private bool isKnockedBack;

    private float lastAttackTime = -Mathf.Infinity;

    private float navAccelerationModifier = 20f;
    private float navAngularModifier = 35;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navAgent = this.GetComponent<NavMeshAgent>();
        enemy = gameObject.GetComponent<Enemy>();
        isKnockedBack = false;


        navAgent.speed = enemy.getMovementSpeed();
        navAgent.angularSpeed = navAgent.speed * navAngularModifier;
        navAgent.acceleration = navAgent.speed * navAccelerationModifier;
    }

    void Update()
    {
        if (GetDistanceFromPlayer() >= enemy.getPursueDistance() && !isKnockedBack) {
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
        //Debug.Log(this + " attacked player at " + Time.time);
        lastAttackTime = Time.time;
        gameObject.GetComponent<Attack>().Perform(player, enemy);
    }

    public void getKnockedBack(AttackInfo info) {
        StartCoroutine(Knockback(info));
    }

    IEnumerator Knockback(AttackInfo info) {
        isKnockedBack = true;
        Vector3 direction = this.transform.position - player.transform.position;
        direction.y = 0.0f;
        navAgent.SetDestination(this.transform.position + direction.normalized * info.knockback);


        navAgent.speed = enemy.getKnockbackSpeed();
        navAgent.angularSpeed = 0;
        navAgent.acceleration = 100;
        
        yield return new WaitForSeconds(0.1f);

        navAgent.speed = enemy.getMovementSpeed();
        navAgent.angularSpeed = navAgent.speed * navAngularModifier;
        navAgent.acceleration = navAgent.speed * navAccelerationModifier;
        isKnockedBack = false;
    }
}
