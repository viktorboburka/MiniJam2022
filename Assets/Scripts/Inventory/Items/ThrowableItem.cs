using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Throwable", menuName = "ScriptableObjects/Items/Throwable", order = 1)]
public class ThrowableItem : Item
{
    public GameObject throwablePrefab;
    public GameObject throwableSplashPrefab;

    public override void Activate(ItemArgs _args)
    {
        base.Activate(_args);

        Throwable throwable = Instantiate(throwablePrefab, _args.shootPoint.position, _args.shootPoint.rotation).GetComponent<Throwable>();
        throwable.itemThrowable = this;
        throwable.throwableSplashPrefab = throwableSplashPrefab;

        throwable.GetComponent<Rigidbody>().AddForce((_args.shootPoint.forward * speed) + (_args.shootPoint.up * speed / 2f), ForceMode.Impulse);
        throwable.GetComponent<Rigidbody>().AddTorque((_args.shootPoint.forward * speed) + (_args.shootPoint.up * speed / 2f), ForceMode.Impulse);
    }
}
