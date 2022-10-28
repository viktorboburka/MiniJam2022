using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    ManualWeapon,
    AutoWeapon
}

public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public ItemType itemType;

    public virtual void Activate(){
        
    }
}
