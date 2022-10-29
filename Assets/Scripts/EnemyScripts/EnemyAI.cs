using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    private GameObject player;
    private NavMeshAgent navAgent;
    private Enemy enemy;

    private bool isKnockedBack = false;

    private float lastAttackTime = -Mathf.Infinity;

    private float navAccelerationModifier = 20f;
    private float navAngularModifier = 35;
    private float navStoppingDist;

    [SerializeField]
    private float randMovementRadius = 3;
    private bool isMovingRandomly = false;
    private Vector3 randomDestination;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navAgent = this.GetComponent<NavMeshAgent>();
        enemy = gameObject.GetComponent<Enemy>();

        navStoppingDist = enemy.getRange() - 1;
        navAgent.speed = enemy.getMovementSpeed();
        navAgent.angularSpeed = navAgent.speed * navAngularModifier;
        navAgent.acceleration = navAgent.speed * navAccelerationModifier;
        navAgent.stoppingDistance = navStoppingDist;
    }

    void Update() {

        if (isKnockedBack) {
            return;
        }

        if (GetDistanceFromPlayer() >= navStoppingDist) {
            navAgent.SetDestination(player.transform.position);
            isMovingRandomly = false;
            navAgent.stoppingDistance = navStoppingDist;
            navAgent.speed = enemy.getMovementSpeed();
        }

        //if enemy has range on player, rotate to face player
        else {
            Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - this.transform.position);
            transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * navAgent.angularSpeed / 10.0f);

            //if player is in range, move randomly. if they are closer than half range, tend to move away
            if (!isMovingRandomly) {
                if (GetDistanceFromPlayer() < enemy.getRange() / 2.0f) {
                    do {
                        randomDestination = GetRandomPointInRadius(randMovementRadius);
                    } while ((randomDestination - player.transform.position).magnitude < GetDistanceFromPlayer());
                }
                else {
                    randomDestination = GetRandomPointInRadius(randMovementRadius);
                    /*do {
                        randomDestination = GetRandomPointInRadius(randMovementRadius);
                    } while ((randomDestination - player.transform.position).magnitude > GetDistanceFromPlayer());*/
                }

                isMovingRandomly = true;
                navAgent.stoppingDistance = 0f;
                navAgent.speed = enemy.getMovementSpeed() / 2.0f;
            }
            if ((transform.position - randomDestination).magnitude < 0.5f) {
                isMovingRandomly = false;
                navAgent.stoppingDistance = navStoppingDist;
                navAgent.speed = enemy.getMovementSpeed();
            }

            navAgent.SetDestination(randomDestination);

        }
        Debug.Log("pos: " + transform.position + " rand: " + randomDestination + " is moving randomly: " + isMovingRandomly + " navdest: " + navAgent.destination + " stopping dist: " + navAgent.stoppingDistance);
        if (PlayerInRange() && ReadyToAttack()) {
            Attack();
        }
    }

    private Vector3 GetRandomPointInRadius(float r) {
        float rx = Random.Range(-r, r);
        float rz = Random.Range(-r, r);

        return this.transform.position + new Vector3(rx, 0, rz);
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
