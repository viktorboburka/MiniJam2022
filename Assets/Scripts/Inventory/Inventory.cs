using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStat{
    public PlayerStat(){
        items.Add(boltCount);
        items.Add(garlicCount);
        items.Add(crossCount);
        items.Add(crossbowCount);
        items.Add(stakeCount);
    }

    public int boltCount = 0;
    public int garlicCount = 0;
    public int crossCount = 0;
    public int crossbowCount = 0;
    public int stakeCount = 0;

    public List<int> items = new();
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private LevelUISystem levelSystem;
    [SerializeField] private int experienceNeeded = 250;
    private int experience = 0;

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

    public void AddItem(Item _item){
        items.Add(_item);

        if(!CheckItemCount(_item))
            onAddedItem.Invoke(_item);
        else
            onUpgradedItem.Invoke(_item);
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

    public int GetItemCount(Item _item){
        switch(_item.itemName){
            case "Bolt":
                    return stats.boltCount;
            case "Cross":
                    return stats.crossCount;
            case "Crossbow":
                    return stats.crossbowCount;
            case "Stake":
                    return stats.stakeCount;
        }

        return 0;
    }

    public int GetItemUniqueCount(){
        int count = 0;

        foreach(int itemCount in stats.items){
            if(itemCount > 0)
                count++;
        }

        return count;
    }

    public void LevelUp(){
        levelSystem.levelUps++;
    }

    public void EXPCollected(int exp){
        experience += exp;
        if(experience >= experienceNeeded){
            experience -= experienceNeeded;
            experienceNeeded = (int)(experienceNeeded * 1.5f);
            LevelUp();
        }
    }
}
