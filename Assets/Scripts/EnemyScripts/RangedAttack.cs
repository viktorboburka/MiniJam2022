using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float playerMovementPrediction = 2f;
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
        Vector3 projectileTarget = player.transform.position + player.transform.GetComponent<Rigidbody>().velocity * playerMovementPrediction;
        GameObject projectile = Instantiate(projectilePrefab, this.transform.position + (projectileTarget - this.transform.position).normalized * 0.1f, this.transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;
        projectile.GetComponent<EnemyProjectile>().info = new AttackInfo(enemy.getDmg(), 0);
    }
}
