using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AOEStationary", menuName = "ScriptableObjects/Items/AOEStationary", order = 1)]
public class AOE : Item
{
    public GameObject AOIPrefab;

    public override void Activate(ItemArgs _args)
    {
        base.Activate(_args);

        Instantiate(AOIPrefab, _args.shootPoint.parent).GetComponent<AOEController>()._item = this;
    }
}
