using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterClean : MonoBehaviour
{

    void OnTriggerEnter(Collider coll){
        if(coll.tag == "Blood"){
            Destroy(coll.gameObject);
        }
    }


    public void DestroyItself() => Destroy(gameObject);
}
