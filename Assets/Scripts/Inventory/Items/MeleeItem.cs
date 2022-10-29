using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee", menuName = "ScriptableObjects/Items/Melee", order = 1)]
public class MeleeItem : Item
{
    public GameObject projectilePrefab;

    public override void Activate(ItemArgs _args)
    {
        base.Activate(_args);

        GameObject dmgBox = Instantiate(projectilePrefab, _args.shootPoint.parent.position + _args.shootPoint.parent.forward * 1.5f, _args.shootPoint.parent.rotation, _args.shootPoint.transform);

        Destroy(dmgBox, 1f);

        dmgBox.GetComponent<Melee>().attackInfo = new AttackInfo(damage + (int)(damage * _args.inventory.GetItemCount(this) * 0.15f), knockback);
    }
}
