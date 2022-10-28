using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<Item> items = new List<Item>();

    void OnTriggerEnter(Collider coll){
        if(coll.tag == "item"){
            coll.GetComponent<Item>();
        }
    }
}
