using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerStat{
    public PlayerStat(){
        items.Add(boltCount);
        items.Add(garlicCount);
        items.Add(crossCount);
        items.Add(crossbowCount);
        items.Add(stakeCount);
        items.Add(dualBladesCount);
    }

    public int boltCount = 0;
    public int garlicCount = 0;
    public int crossCount = 0;
    public int crossbowCount = 0;
    public int stakeCount = 0;
    public int dualBladesCount = 0;

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
    [SerializeField] private int level = 1;
    [SerializeField] private int experienceNeeded = 250;
    private int experience = 0;

    [SerializeField] public List<Item> items = new List<Item>();
    [SerializeField] private UnityEvent<Item> onAddedItem;
    [SerializeField] private UnityEvent<Item> onUpgradedItem;
    [SerializeField] public PlayerStat stats = new PlayerStat();
    
    [SerializeField] public Image damageIndicator;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float cooldownTime = 0.5f;

    
    [SerializeField] public Image healIndicator;
    [SerializeField] private float cooldownHeal = 0.5f;
    [SerializeField] private float cooldownTimeHeal = 0.5f;

    
    [SerializeField] public InputManager inputManager;
    [SerializeField] private InputAction debugRestart;

    //Water Logic
    [SerializeField] private GameObject water;
    [SerializeField] private InputAction waterAction;
    [SerializeField] private float waterCooldown = 9;
    [SerializeField] private float waterCooldownTime;
    [SerializeField] private RectTransform cooldownBar;
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private Canvas canvas;
    
    [SerializeField] private Animator UIAnim;

    [SerializeField] private GameObject dmgSound;
    [SerializeField] private GameObject xpSound;
    [SerializeField] private GameObject healSound;

    void playSound(GameObject sound)
    {
        Instantiate(sound, transform);
    }


    void OnEnable(){

        inputManager.Enable();

        debugRestart = inputManager.Player.DebugRestart;
        debugRestart.Enable();
        debugRestart.performed += RestartSceneDebug;

        waterAction = inputManager.Player.WaterAction;
        waterAction.Enable();
        waterAction.performed += CastWater;
    }

    void OnDisable(){
        inputManager.Disable();
        debugRestart.Disable();
    }

    void Awake(){
        inputManager = new();
        health = maxHealth;
        canvas = GetComponentInChildren<Canvas>();
    }

    void Update(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f)){
            if(hit.transform.tag == "Blood" && canBloodDmg){
                StartCoroutine(BloodCooldown());
                GetDamaged(hit.collider.GetComponent<BloodSplat>().damage);
            }
        }

        if(waterCooldownTime > 0){
            waterText.text = Mathf.Floor(waterCooldownTime).ToString();
            Transform a = cooldownBar.transform.parent;
            cooldownBar.transform.SetParent(null, false);
            cooldownBar.localScale = new Vector3(1f, waterCooldownTime / ((200 / (level + waterCooldown)) + 1), 1f);
            cooldownBar.transform.SetParent(a, false);
            waterCooldownTime -= Time.deltaTime;
        }

        if(cooldownTime < cooldown){
            cooldownTime += Time.deltaTime;
            damageIndicator.color = new Color(1f, 1f, 1f, 0.75f - Mathf.Clamp(1f * (cooldownTime / cooldown), 0f, 0.75f));
        }

        if(cooldownTimeHeal < cooldownHeal){
            cooldownTimeHeal += Time.deltaTime;
            healIndicator.color = new Color(healIndicator.color.r, healIndicator.color.g, healIndicator.color.b, 0.3f - Mathf.Clamp(1f * (cooldownTimeHeal / cooldownHeal), 0f, 0.3f));
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

    public void CastWater(InputAction.CallbackContext context){
        if(waterCooldownTime <= 0){
            Instantiate(water, transform.position - Vector3.up * 0.9f, transform.rotation);
            waterCooldownTime = (200 / (level + waterCooldown)) + 1;
        }
    }

    public void GetDamaged(int dmg){
        cooldownTime = 0;
        health -= dmg;

        playSound(dmgSound);

        if (health <= 0){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            health = 0;
            Time.timeScale = 0;
            UIAnim.SetBool("isDead", true);
        }
    }

    public void GetHealed(int heal){
        cooldownTimeHeal = 0;

        playSound(healSound);

        if (health + heal > maxHealth)
            health = maxHealth;
        else
            health += heal;
    }

    public void LevelHeal(){

        playSound(healSound);

        if (health + (maxHealth * 0.2f) > maxHealth)
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
            case "Dual Blades":
                stats.dualBladesCount++;
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
            case "Dual Blades":
                if(stats.dualBladesCount > 0)
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
            case "Dual Blades":
                    return stats.dualBladesCount;
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
            case "Dual Blades":
                    return (100f/(GetItemCount(_item) + _item.cooldown)) / 10f; // 2
        }

        return 1f;
    }

    public void EXPCollected(int exp){
        experience += exp;

        playSound(xpSound);

        if (experience >= experienceNeeded){
            level++;
            experience -= experienceNeeded;
            experienceNeeded += (int)((experienceNeeded * level) * 0.01f);
            LevelUp();
        }
    }

    public void RestartGame(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MainMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public int GetHP() { return health; }
    public int GetMaxHP() { return maxHealth; }
    public int GetExperience() { return experience; }
    public int GetExperienceNeeded() { return experienceNeeded; }

}
