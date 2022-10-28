using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    ManualWeapon,
    AutoWeapon
}

[System.Serializable]
public class ItemArgs {
    public Transform shootPoint;
}

public class Item : ScriptableObject
{
    //Information
    public string itemName;
    public Sprite itemSprite;
    public ItemType itemType;

    //Stats
    public float cooldown;
    public float damage;
    public float speed;

    //Functions
    public virtual void Activate(ItemArgs _args){
        
    }
    
    public virtual void Activate(){
        
    }
}
