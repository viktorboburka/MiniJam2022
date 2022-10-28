using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerAbilitySystem AbilitySys;
    [SerializeField] private GameObject itemIcons;
    [SerializeField] private Image[] images;


    private void Start()
    {
        var count = itemIcons.transform.childCount;
        images = new Image[count];
        for (int i = 0; i < count; ++i)
        {
            images[i] = itemIcons.transform.GetChild(i).GetComponent<Image>();
        }

    }

    public void NewSlotWasAdded() {
        var slotCount = AbilitySys.Slots.Count;

        for(int i = 0; i < slotCount; i++)
        {
            images[i].color = Color.white;
            images[i].sprite = AbilitySys.Slots[i].item.itemSprite;
        }

    }


    // Update is called once per frame
    void Update()
    {
        var slots = AbilitySys.Slots;
        var slotCount = slots.Count;
        for (int i = 0; i < slotCount; i++)
        { 
            if(slots[i].CooldownTime > 0)
            {
                var newColorIndex = 1 - Mathf.Clamp(slots[i].CooldownTime / slots[i].item.cooldown, 0, 1);
                images[i].color = new Color(1, newColorIndex, newColorIndex);
            }
        }
    }
}
