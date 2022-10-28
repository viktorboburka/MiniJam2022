using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    ManualWeapon,
    AutoWeapon,
    AOEWeapon
}

[System.Serializable]
public class ItemArgs {
    public Transform shootPoint;
    public Inventory inventory;
}

public class Item : ScriptableObject
{
    //Information
    public string itemName;
    public Sprite itemSprite;
    public ItemType itemType;

    //Stats
    public float cooldown;
    public int damage;
    public float speed;
    public float knockback;

    //Functions
    public virtual void Activate(ItemArgs _args){
        
    }

    public virtual void Activate(){
        
    }
}
