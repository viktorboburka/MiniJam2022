using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitySystem : MonoBehaviour
{
    public AbilitieList Abilities = new();
    public int MaxItems { get; set; } = 5;

    #region mockKeyBinds
    bool mockKeyBindLeftClick = false;
    public bool MockKeyBindLeftClick {
        get
        {
            bool returnValue = mockKeyBindLeftClick;
            mockKeyBindLeftClick = false;
            return returnValue;
        }
        set
        {
            mockKeyBindLeftClick = value;
        }    
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mockKeyBindLeftClick)
        {
            Abilities.GetAbility(KeyCode.Mouse0).Activate();
        }


    }



}