using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField]
    private float projectileSpeed;
    //[SerializeField]
    private float playerMovementPrediction = 1f;
    [SerializeField]
    GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Perform(GameObject player, Enemy enemy) {
        //TODO: play animation & sound

        playerMovementPrediction = GetDistanceFromPlayer(player) / projectileSpeed;
        Vector3 projectileTarget = player.transform.position + player.transform.GetComponent<Rigidbody>().velocity * playerMovementPrediction;

        GameObject projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.LookRotation(projectileTarget - this.transform.position, Vector3.up));
        projectile.GetComponent<Rigidbody>().velocity = (projectileTarget - this.transform.position).normalized * projectileSpeed;
        projectile.GetComponent<EnemyProjectile>().info = new AttackInfo(enemy.getDmg(), 0);
    }

    private float GetDistanceFromPlayer(GameObject player) {
        return Vector3.Distance(this.transform.position, player.transform.position);
    }
}
