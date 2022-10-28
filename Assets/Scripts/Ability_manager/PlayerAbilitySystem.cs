using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilitySystem : MonoBehaviour
{
    [SerializeField]
    public AbilitieList Abilities = new();
    [SerializeField]
    public ItemArgs argumets = new();


    #region mockKeyBinds
    [SerializeField]
    bool mockKeyBindLeftClick = false;
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
            mockKeyBindLeftClick = false;
            Abilities.GetAbility(KeyCode.Mouse0).Activate(argumets);
        }


    }



}