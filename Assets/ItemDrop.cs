using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private SpriteRenderer spriteRend;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend.sprite = item.itemSprite;
    }

    public Item GetItem() { return item; }
}
