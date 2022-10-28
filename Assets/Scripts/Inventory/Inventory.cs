using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    void OnTriggerEnter(Collider coll){
        Debug.Log("Entered");
        if(coll.tag == "Item"){
            items.Add(coll.GetComponent<ItemDrop>().GetItem());
            Destroy(coll.gameObject);
        }
    }
}
