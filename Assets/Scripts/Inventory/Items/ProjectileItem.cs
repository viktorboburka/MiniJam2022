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

        Instantiate(projectilePrefab, _args.shootPoint.position, _args.shootPoint.rotation);
    }
}
