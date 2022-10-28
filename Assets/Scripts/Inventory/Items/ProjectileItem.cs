using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Items/Projectile", order = 1)]
public class ProjectileItem : Item
{
    public GameObject projectilePrefab;

    public override void Activate(ItemArgs _args)
    {
        base.Activate(_args);

        GameObject projectile = Instantiate(projectilePrefab, _args.shootPoint.position, _args.shootPoint.rotation);

        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * speed;
    }
}
