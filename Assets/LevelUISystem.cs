using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUISystem : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Item[] itemsPool;
    [SerializeField] private Animator anim;
    
    [SerializeField] private LevelItemUI[] itemsUI;
    
    [SerializeField] private bool DebugLevelUp = false;

    private void Update(){
        if(DebugLevelUp){
            StartLevelingPrompt();
            DebugLevelUp = false;
        }
    }

    public void StartLevelingPrompt(){
        foreach(LevelItemUI itemLevel in itemsUI){
            itemLevel.UpdateItem(GenerateItem());
        }

        Cursor.lockState = CursorLockMode.None;
        anim.SetBool("isOpen", true);
    }

    private Item GenerateItem(){
        if(inventory.GetItemUniqueCount() < 5){
            return itemsPool[Random.Range(0, itemsPool.Length)];
        }

        return inventory.items[Random.Range(0, inventory.items.Count)];
    }
}
