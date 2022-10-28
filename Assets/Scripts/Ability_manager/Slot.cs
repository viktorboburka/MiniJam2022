using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

[System.Serializable]
public class Slot
{
    [SerializeField]
    public Item item;
    public InputAction KeyBind;
    public float CooldownTime = 0f;
}