using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Events;

public class PlayerAbilitySystem : MonoBehaviour
{
    public List<Slot> Slots = new();
    public UnityEvent onAddedItem;


    [SerializeField] private ItemArgs ItemArgumets = new();
    [SerializeField] private InputActionAsset actions;
    
    [SerializeField] private Inventory inventory;

    private List<Slot> activatedSlots = new();


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void OnEnable()
    {
    }


    /// <summary>
    /// called when new item is added to inventory
    /// </summary>
    public void AddedItem(Item item)
    {
        if (Slots.Count >= 5)
        {
            Debug.Log("Maximum items achived");
            return;
        }

        Item newItem = item;
        var newSlot = new Slot() { item = newItem };
        Slots.Add(newSlot);

        if (newItem.itemType == ItemType.ManualWeapon)
        {
            //gets number of manual wepons
            var position = Slots.FindAll(slot => slot.item.itemType == ItemType.ManualWeapon).Count;
            newSlot.KeyBind = actions["Player/Abillity" + position];
            newSlot.KeyBind.Enable();
            //newSlot.KeyBind.performed += ctx => ActivateItem(newSlot);
            newSlot.KeyBind.started += ctx => StartActivating(newSlot);
            newSlot.KeyBind.canceled += ctx => StopActivating(newSlot);
        }
        else if (newItem.itemType == ItemType.AOEWeapon)
        {
            newItem.Activate(ItemArgumets);
        }
        else if (newItem.itemType == ItemType.AutoWeapon)
        {
            activatedSlots.Add(newSlot);
        }

        onAddedItem.Invoke();
    }
    private void StartActivating(Slot slot)
    {
        activatedSlots.Add(slot);
    }

    private void StopActivating(Slot slot)
    {
        activatedSlots.Remove(slot);
    }

    private void ActivateItem()
    {
        foreach(Slot slot in activatedSlots)
        {
            if (slot.CooldownTime > 0)
                continue;
            slot.item.Activate(ItemArgumets);
            slot.CooldownTime = inventory.GetCooldown(slot.item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //cooling down
        Slots
            .FindAll(slot => slot.CooldownTime > 0)
            .ForEach(slot => slot.CooldownTime -= Time.deltaTime);

        ActivateItem();


    }



}