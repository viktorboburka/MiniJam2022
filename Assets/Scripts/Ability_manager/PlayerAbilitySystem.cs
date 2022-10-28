using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilitySystem : MonoBehaviour
{
    [SerializeField]
    public Abilities Abilities = new();
    [SerializeField]
    public ItemArgs ItemArgumets = new();

    public InputManager inputManager;


    void Awake()
    {
        inputManager = new InputManager();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    private void OnEnable()
    {
        inputManager.Enable();

        for (int i = 0; i < Abilities.Slots.Count; i++)
        {
            Abilities.Slots[i].KeyBind = inputManager.FindAction("Player/Abillity" + (i + 1));
            Abilities.Slots[i].KeyBind.Enable();
            var activatedSlot = Abilities.Slots[i];
            Abilities.Slots[i].KeyBind.performed += ctx => ActivateItem(activatedSlot);

        }
    }

    private void ActivateItem(Slot slot)
    {
        if (slot.CooldownTime > 0)
            return;
        slot.item.Activate(ItemArgumets);
        slot.CooldownTime = slot.item.cooldown;
    }



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //cooldown
        Abilities.Slots
            .FindAll(slot => slot.CooldownTime > 0)
            .ForEach(slot => slot.CooldownTime -= Time.deltaTime);
    }



}