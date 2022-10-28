using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AutoProjectile", menuName = "ScriptableObjects/Items/AutoProjectile", order = 1)]
public class AutoItem : Item
{
    public GameObject projectilePrefab;

    public override void Activate(ItemArgs _args)
    {
        base.Activate(_args);

        Debug.Log(_args.inventory);
        if(_args.inventory.transform.GetComponentInChildren<Radar>().closestEnemy == null)
            return;

        Transform enemy = _args.inventory.transform.GetComponentInChildren<Radar>().closestEnemy.transform;

        GameObject projectile = Instantiate(projectilePrefab, _args.inventory.transform.position + (Vector3.up * 2f), Quaternion.LookRotation(enemy.position - (_args.inventory.transform.position + (Vector3.up * 2f))));

        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * speed;
        projectile.GetComponent<Projectile>().attackInfo = new AttackInfo(damage + (int)((damage * (_args.inventory.GetItemCount(this))) * 0.15f), knockback);
    }
}
