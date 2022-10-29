using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public Item itemThrowable;
    public GameObject throwableSplashPrefab;
    public float offset = 1.1f;

    void OnTriggerEnter(Collider coll){
        if(coll.tag == "World"){
            AOEController aoe = Instantiate(throwableSplashPrefab, transform.position + Vector3.up * offset, Quaternion.identity).GetComponent<AOEController>();
            aoe._item = itemThrowable;
            Destroy(gameObject);
        }
    }
}
