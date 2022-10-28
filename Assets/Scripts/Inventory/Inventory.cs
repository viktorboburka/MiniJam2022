using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStat{
    public int boltCount = 0;
    public int garlicCount = 0;
    public int crossCount = 0;
    public int crossbowCount = 0;
    public int stakeCount = 0;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] public List<Item> items = new List<Item>();
    [SerializeField] private UnityEvent<Item> onAddedItem;
    [SerializeField] private UnityEvent<Item> onUpgradedItem;
    [SerializeField] public PlayerStat stats = new PlayerStat();

    void OnTriggerEnter(Collider coll){
        Debug.Log("Entered");
        if(coll.tag == "Item"){
            items.Add(coll.GetComponent<ItemDrop>().GetItem());

            if(!CheckItemCount(coll.GetComponent<ItemDrop>().GetItem()))
                onAddedItem.Invoke(coll.GetComponent<ItemDrop>().GetItem());
            else
                onUpgradedItem.Invoke(coll.GetComponent<ItemDrop>().GetItem());
            
            Destroy(coll.gameObject);
        }
    }

    public void AddItemCount(Item _item){
        switch(_item.itemName){
            case "Bolt":
                stats.boltCount++;
                break;
            case "Cross":
                stats.crossCount++;
                break;
            case "Crossbow":
                stats.crossbowCount++;
                break;
            case "Stake":
                stats.stakeCount++;
                break;
        }
    }

    public bool CheckItemCount(Item _item){
        switch(_item.itemName){
            case "Bolt":
                if(stats.boltCount > 0)
                    return true;
                else
                    return false;
            case "Cross":
                if(stats.crossCount > 0)
                    return true;
                else
                    return false;
            case "Crossbow":
                if(stats.crossbowCount > 0)
                    return true;
                else
                    return false;
            case "Stake":
                if(stats.stakeCount > 0)
                    return true;
                else
                    return false;
        }

        return false;
    }
}
