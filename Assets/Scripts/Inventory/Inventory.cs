using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] public List<Item> items = new List<Item>();
    [SerializeField] public UnityEvent<Item> onAddedItem;

    void OnTriggerEnter(Collider coll){
        Debug.Log("Entered");
        if(coll.tag == "Item"){
            items.Add(coll.GetComponent<ItemDrop>().GetItem());
            onAddedItem.Invoke(coll.GetComponent<ItemDrop>().GetItem());
            Destroy(coll.gameObject);
        }
    }
}
