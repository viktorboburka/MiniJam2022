using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AbilitieList : List<Item>
{
    public readonly KeyCode[] ControlKeys =
    {
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Q,
        KeyCode.E,
        KeyCode.R
    };


    public KeyCode GetKeyBind(Item item)
    {
        if (!Contains(item))
            return KeyCode.None;

        return ControlKeys[IndexOf(item)];
    }


    public Item GetAbility(KeyCode keyPress)
    {
        for( int i = 0; i < ControlKeys.Length; i++)
        {
            if (ControlKeys[i] == keyPress)
                return this[i];
        }
        return default;
    }

}
