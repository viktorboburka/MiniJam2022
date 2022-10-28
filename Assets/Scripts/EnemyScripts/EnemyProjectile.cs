using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public AttackInfo info;

    void OnTriggerEnter(Collider coll) {
        if (coll.tag == "projectile" || coll.tag == "Enemy")
            return;

        if (coll.tag == "Player") {
            coll.SendMessageUpwards("getAttacked", info); // add HitArguments
        }
        Destroy(gameObject);
    }
}
