using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class AbilitieList 
{
    public List<Item> items;
    [SerializeField]
    public KeyCode[] ControlKeys =
    {
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Q,
        KeyCode.E,
        KeyCode.R
    };


    public KeyCode GetKeyBind(Item item)
    {
        if (!items.Contains(item))
            return KeyCode.None;

        return ControlKeys[items.IndexOf(item)];
    }


    public Item GetAbility(KeyCode keyPress)
    {
        for( int i = 0; i < ControlKeys.Length; i++)
        {
            if (ControlKeys[i] == keyPress)
                return items[i];
        }
        return default;
    }

}
