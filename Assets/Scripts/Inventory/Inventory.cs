using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerStat{
    public PlayerStat(){
        items.Add(boltCount);
        items.Add(garlicCount);
        items.Add(crossCount);
        items.Add(crossbowCount);
        items.Add(stakeCount);
        items.Add(dualBladeCount);
    }

    public int boltCount = 0;
    public int garlicCount = 0;
    public int crossCount = 0;
    public int crossbowCount = 0;
    public int stakeCount = 0;
    public int dualBladeCount = 0;

    public List<int> items = new();
}

public class Inventory : MonoBehaviour
{
    //HP SYSTEM + LEVELING HEALING TO-DO
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;

    [SerializeField] private float bloodCooldown = 0.6f;
    [SerializeField] private bool canBloodDmg = true;

    [SerializeField] private LevelUISystem levelSystem;
    [SerializeField] private int experienceNeeded = 250;
    private int experience = 0;

    [SerializeField] public List<Item> items = new List<Item>();
    [SerializeField] private UnityEvent<Item> onAddedItem;
    [SerializeField] private UnityEvent<Item> onUpgradedItem;
    [SerializeField] public PlayerStat stats = new PlayerStat();

    
    [SerializeField] public InputManager inputManager;
    [SerializeField] private InputAction debugRestart;

    void OnEnable(){
        inputManager.Enable();

        debugRestart = inputManager.Player.DebugRestart;
        debugRestart.Enable();
        debugRestart.performed += RestartSceneDebug;
    }

    void OnDisable(){
        inputManager.Disable();
        debugRestart.Disable();
    }

    void Awake(){
        inputManager = new();
        health = maxHealth;
    }

    void Update(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f)){
            if(hit.transform.tag == "Blood" && canBloodDmg){
                StartCoroutine(BloodCooldown());
                GetDamaged(hit.collider.GetComponent<BloodSplat>().damage);
            }
        }
    }

    public void RestartSceneDebug(InputAction.CallbackContext context){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator BloodCooldown(){
        canBloodDmg = false;
        yield return new WaitForSeconds(bloodCooldown);
        canBloodDmg = true;
    }

    void OnTriggerEnter(Collider coll){
        if(coll.tag == "Item"){
            items.Add(coll.GetComponent<ItemDrop>().GetItem());

            if(!CheckItemCount(coll.GetComponent<ItemDrop>().GetItem()))
                onAddedItem.Invoke(coll.GetComponent<ItemDrop>().GetItem());
            else
                onUpgradedItem.Invoke(coll.GetComponent<ItemDrop>().GetItem());
            
            Destroy(coll.gameObject);
        }
    }

    public void GetDamaged(int dmg){
        health -= dmg;
        if(health <= 0){
            health = 0;
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    public void GetHealed(int heal){
        if(health + heal > maxHealth)
            health = maxHealth;
        else
            health += heal;
    }

    public void LevelHeal(){
        if(health + (maxHealth * 0.2f) > maxHealth)
            health = maxHealth;
        else
            health += (int)(maxHealth * 0.2f);
    }

    public void AddItem(Item _item){
        items.Add(_item);

        if(!CheckItemCount(_item))
            onAddedItem.Invoke(_item);
        else
            onUpgradedItem.Invoke(_item);
    }

    public void AddItemCount(Item _item){
        switch(_item.itemName){
            case "Bolt":
                stats.boltCount++;
                stats.items[0]++;
                break;
            case "Garlic":
                stats.garlicCount++;
                stats.items[1]++;
                break;
            case "Cross":
                stats.crossCount++;
                stats.items[2]++;
                break;
            case "Crossbow":
                stats.crossbowCount++;
                stats.items[3]++;
                break;
            case "Stake":
                stats.stakeCount++;
                stats.items[4]++;
                break;
            case "Dual Blade":
                stats.stakeCount++;
                stats.items[5]++;
                break;
        }
    }

    public bool CheckItemCount(Item _item){
        switch(_item.itemName){
            case "Bolt":
                if(stats.boltCount > 0)
                    return true;
                else
                    return false;
            case "Garlic":
                if(stats.garlicCount > 0)
                    return true;
                else
                    return false;
            case "Cross":
                if(stats.crossCount > 0)
                    return true;
                else
                    return false;
            case "Crossbow":
                if(stats.crossbowCount > 0)
                    return true;
                else
                    return false;
            case "Stake":
                if(stats.stakeCount > 0)
                    return true;
                else
                    return false;
            case "Dual Blade":
                if(stats.stakeCount > 0)
                    return true;
                else
                    return false;
        }

        return false;
    }

    public int GetItemCount(Item _item){
        switch(_item.itemName){
            case "Bolt":
                    return stats.boltCount;
            case "Garlic":
                    return stats.garlicCount;
            case "Cross":
                    return stats.crossCount;
            case "Crossbow":
                    return stats.crossbowCount;
            case "Stake":
                    return stats.stakeCount;
            case "Dual Blade":
                    return stats.stakeCount;
        }

        return 0;
    }

    public int GetItemUniqueCount(){
        int count = 0;

        foreach(int itemCount in stats.items){
            if(itemCount > 0)
                count++;
        }

        return count;
    }

    public void LevelUp(){
        levelSystem.levelUps++;
    }

    public float GetCooldown(Item _item){
        switch(_item.itemName){
            case "Bolt":
                    return (100f/(GetItemCount(_item) + _item.cooldown)) / 20f; // cooldown 2
            case "Garlic":
                    return ((100f/(GetItemCount(_item) + _item.cooldown)) / 10f) + 2f; // cooldown 2
            case "Cross":
                    return stats.crossCount;
            case "Crossbow":
                    return (100f/(GetItemCount(_item) + _item.cooldown)) / 20f; // cooldown 1
            case "Stake":
                    return (100f/(GetItemCount(_item) + _item.cooldown)) / 10f; // 3
            case "Dual Blade":
                    return (100f/(GetItemCount(_item) + _item.cooldown)) / 10f; // 2
        }

        return 1f;
    }

    public void EXPCollected(int exp){
        experience += exp;
        if(experience >= experienceNeeded){
            experience -= experienceNeeded;
            experienceNeeded = (int)(experienceNeeded * 1.5f);
            LevelUp();
        }
    }

    public int GetHP() { return health; }
    public int GetMaxHP() { return maxHealth; }
    public int GetExperience() { return experience; }
    public int GetExperienceNeeded() { return experienceNeeded; }

}
