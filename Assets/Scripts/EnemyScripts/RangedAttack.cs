using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField]
    private float projectileSpeed;
    [SerializeField] private bool enableProjectilePrediction;
    //[SerializeField]
    private float playerMovementPrediction = 1f;
    [SerializeField]
    GameObject projectilePrefab;

    public override void Perform(GameObject player, Enemy enemy) {
        //TODO: play animation & sound

        //playerMovementPrediction = GetDistanceFromPlayer(player) / projectileSpeed;
        Vector3 projectileTarget = player.transform.position - transform.position; // + player.transform.GetComponent<Rigidbody>().velocity * playerMovementPrediction;

        GameObject projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.LookRotation(projectileTarget, Vector3.up));
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward  * projectileSpeed;
        projectile.GetComponent<EnemyProjectile>().info = new AttackInfo(enemy.getDmg(), 0);
    }

    private float GetDistanceFromPlayer(GameObject player) {
        return Vector3.Distance(this.transform.position, player.transform.position);
    }
}
