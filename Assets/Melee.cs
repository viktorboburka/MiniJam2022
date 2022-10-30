using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public AttackInfo attackInfo;

    List<Enemy> enemies = new List<Enemy>();

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Projectile") || coll.CompareTag("Item") || coll.CompareTag("Player"))
            return;

        if (coll.CompareTag("Enemy") && !enemies.Contains(coll.GetComponent<Enemy>()))
        {
            enemies.Add(coll.GetComponent<Enemy>());
            coll.SendMessageUpwards("getAttacked", attackInfo, SendMessageOptions.DontRequireReceiver); // add HitArguments
        }
    }

    public void DestroyItself(){
        Destroy(gameObject);
    }
}
