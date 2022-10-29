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

    public int levelUps = 0;
    private bool canLevelUp = true;
    public float timeScaleDefault = 0;

    private void Awake(){
        timeScaleDefault = Time.timeScale;
    }

    private void Update(){
        if(DebugLevelUp){
            levelUps++;
            DebugLevelUp = false;
        }

        if(levelUps > 0 && canLevelUp){
            canLevelUp = false;
            levelUps--;
            StartLevelingPrompt();
        }
    }

    public void StartLevelingPrompt(){
        GetComponentInParent<MovementComponent>().SetLock(true);

        Time.timeScale = 0;
        foreach(LevelItemUI itemLevel in itemsUI){
            itemLevel.UpdateItem(GenerateItem());
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        anim.SetBool("isOpen", true);
    }

    private Item GenerateItem(){
        if(inventory.GetItemUniqueCount() < 5){
            return itemsPool[Random.Range(0, itemsPool.Length)];
        }

        return inventory.items[Random.Range(0, inventory.items.Count)];
    }

    public void ConfirmLevelUp(){
        canLevelUp = true;
        Time.timeScale = timeScaleDefault;
        GetComponentInParent<MovementComponent>().SetLock(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        anim.SetBool("isOpen", false);
    }
}
