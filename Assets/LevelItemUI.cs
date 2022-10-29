using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelItemUI : MonoBehaviour
{
    [SerializeField] private Button btnSelf;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Animator animPrompt;

    [SerializeField] public Item item;
    
    [SerializeField] private Inventory inventory;

    public void UpdateItem(Item _item){
        btnSelf.enabled = true;
        item = _item;

        itemName.text = item.itemName;
        itemImage.sprite = item.itemSprite;
        itemDescription.text = item.itemDescription;
    }

    public void SelectedItem(){
        Cursor.lockState = CursorLockMode.Locked;
        btnSelf.enabled = false;
        inventory.AddItem(item);
        animPrompt.SetBool("isOpen", false);
    }
}
