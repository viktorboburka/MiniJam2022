using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

[System.Serializable]
public class Abilities 
{
    [SerializeField]
    public List<Slot> Slots = new();


    public KeyCode GetKeyBind(Item item)
    {
        //if (!items.Contains(item))
        //    return KeyCode.None;
        throw new System.Exception();
       // return Slots[items.IndexOf(item)].KeyBind;
    }


    public Item GetAbility(KeyCode keyPress)
    {
        throw new System.Exception();

        //for ( int i = 0; i < Slots.Length; i++)
        //{
        //    if (Slots[i].KeyBind == keyPress)
        //        return items[i];
        //}
        return default;
    }

}


[System.Serializable]
public class Slot
{
    [SerializeField]
    public Item item;
    public InputAction KeyBind;
    public float CooldownTime = 0f;
}