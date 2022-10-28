using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public AttackInfo attackInfo;

    void OnTriggerEnter(Collider coll){
        if(coll.tag == "Projectile")
            return;

        if(coll.tag == "Enemy"){
            coll.SendMessageUpwards("getAttacked", attackInfo, SendMessageOptions.DontRequireReceiver); // add HitArguments
        }

        Destroy(gameObject);
    }
}
