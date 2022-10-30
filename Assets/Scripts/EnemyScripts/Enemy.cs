using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private int hp;
    [SerializeField]
    private int maxHp = 10;
    [SerializeField]
    private float movementSpeed = 3.5f;
    [SerializeField]
    private float range;
    [SerializeField]
    private int dmg;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float randMovementRadius = 3f;
    [SerializeField]
    private float idleDistance = 40f;

    private float knockbackSpeed = 100.0f;


    [Header("Drops")]
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private GameObject xpPrefab;
    [SerializeField] private int xpDropAmount = 1;
    [SerializeField] private GameObject hpPrefab;
    [SerializeField] private int hpDropAmount = 0;
    [SerializeField] private int hpDropChance = 0;
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private int itemDropChance;
    
    [SerializeField]
    private SpriteRenderer rend;
    [SerializeField]
    private float cooldown = 0.5f;
    [SerializeField]
    private float cooldownTime = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        InitVariables();
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldownTime < cooldown){
            cooldownTime += Time.deltaTime;
            rend.color = new Color(1f, Mathf.Clamp(1f * (cooldownTime / cooldown), 0f, 1f), Mathf.Clamp(1f * (cooldownTime / cooldown), 0f, 1f), 1f);
        }
    }

    //returns true if this unit got killed
    public void getAttacked(AttackInfo info) {
        //Debug.Log(this + " getAttacked called");

        hp -= info.dmg;
        cooldownTime = 0f;

        //TODO: animation and sound feedback
        if (hp <= 0) {
            // Blood Drop
            Instantiate(bloodPrefab, transform.position + (Vector3.up * 0.25f), Quaternion.Euler(-90, 0, 0));

            // XP Drop
            GameObject xpObj = Instantiate(xpPrefab, transform.position + (Vector3.up * 0.25f), Quaternion.Euler(-90, 0, 0));
            xpObj.GetComponent<Experience>().experiencePoints = xpDropAmount;

            // HP Drop
            if (hpDropChance > 0 && hpDropAmount > 0 && hpPrefab != null)
            {
                if(Random.Range(0, hpDropChance) == 0)
                {

                    GameObject hpObj = Instantiate(hpPrefab, transform.position + (Vector3.up * 0.25f), Quaternion.Euler(-90, 0, 0));
                    hpObj.GetComponent<Heal>().healPoints = hpDropAmount;

                }
            }

            // Item Drop
            if (itemDropChance > 0  && itemPrefabs.Length > 0)
            {
                if(Random.Range(0, itemDropChance) == 0)
                {
                    int itemId = Random.Range(0, itemPrefabs.Length);
                    Instantiate(itemPrefabs[itemId], transform.position + (Vector3.up * 0.25f), Quaternion.Euler(-90, 0, 0));
                }
            }

            Destroy(gameObject);
            return;
        }

        gameObject.GetComponent<EnemyAI>().getKnockedBack(info);
    }


    private void InitVariables() {
        gameObject.GetComponent<NavMeshAgent>().speed = movementSpeed;
        hp = maxHp;
    }


    public float getRange() {
        return range;
    }

    public float getAttackSpeed() {
        return attackSpeed;
    }

    public float getMovementSpeed() {
        return movementSpeed;
    }

    public float getKnockbackSpeed() {
        return knockbackSpeed;
    }

    public int getDmg() {
        return dmg;
    }

    public float getRandMovementRadius() {
        return randMovementRadius;
    }

    public float getIdleDistance() {
        return idleDistance;
    }

}
