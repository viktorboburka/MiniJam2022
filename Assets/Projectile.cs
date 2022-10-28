using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnTriggerEnter(Collider coll){
        if(coll.tag == "Enemy"){
            coll.SendMessageUpwards("Hit");
        }
        Destroy(gameObject);
    }
}
