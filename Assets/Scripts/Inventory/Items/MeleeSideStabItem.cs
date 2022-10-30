using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SideMelee", menuName = "ScriptableObjects/Items/SideMelee", order = 1)]
public class MeleeSideStabItem : Item
{
    public GameObject projectilePrefab;

    public override void Activate(ItemArgs _args)
    {
        base.Activate(_args);

        GameObject dmgBox = Instantiate(projectilePrefab, _args.shootPoint.parent.position, _args.shootPoint.parent.rotation, _args.shootPoint.transform);


        Destroy(dmgBox, timeToDestroy);

        dmgBox.GetComponent<Melee>().attackInfo = new AttackInfo(damage + (int)(damage * _args.inventory.GetItemCount(this) * 0.15f), knockback);
    }
}
