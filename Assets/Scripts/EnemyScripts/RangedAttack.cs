using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField]
    private float projectileSpeed;
    [SerializeField] private Transform projectileSpawnT;
    //[SerializeField]
    private float playerMovementPrediction = 1f;
    [SerializeField]
    GameObject projectilePrefab;

    public override void Perform(GameObject player, Enemy enemy) {
        //TODO: play animation & sound


        Vector3 projectileTarget = player.transform.position + player.transform.GetComponent<Rigidbody>().velocity * playerMovementPrediction;
        
        Vector3 projectileSpawnPos = this.transform.position;
        Quaternion projectileSpawnRot = Quaternion.LookRotation(projectileTarget - this.transform.position, Vector3.up);

        projectileSpawnT.LookAt(player.transform);
        projectileSpawnPos = projectileSpawnT.position;
        projectileSpawnRot = projectileSpawnT.rotation;



        playerMovementPrediction = GetDistanceFromPlayer(player) / projectileSpeed;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPos, projectileSpawnRot);
        projectile.GetComponent<Rigidbody>().velocity = projectileSpawnT.forward * projectileSpeed;

        projectile.GetComponent<EnemyProjectile>().info = new AttackInfo(enemy.getDmg(), 0);
    }

    private float GetDistanceFromPlayer(GameObject player) {
        return Vector3.Distance(this.transform.position, player.transform.position);
    }
}
