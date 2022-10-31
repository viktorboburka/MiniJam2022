using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private SpriteRenderer spriteRend;
    public UnityEvent onPickup;

    void OnDestroy(){
        onPickup.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRend.sprite = item.itemSprite;
    }

    public Item GetItem() { return item; }
}
