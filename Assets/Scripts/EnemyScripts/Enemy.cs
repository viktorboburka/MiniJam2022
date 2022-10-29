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

    private float knockbackSpeed = 100.0f;

    [SerializeField]
    private GameObject[] drops;

    // Start is called before the first frame update
    void Start()
    {
        InitVariables();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //returns true if this unit got killed
    public bool getAttacked(AttackInfo info) {
        //Debug.Log(this + " getAttacked called");

        hp -= info.dmg;
        gameObject.GetComponent<EnemyAI>().getKnockedBack(info);

        //TODO: animation and sound feedback
        if (hp <= 0) {
            foreach(GameObject drop in drops) {
                Instantiate(drop, transform.position + (Vector3.up * 0.25f), Quaternion.identity);
            }

            Destroy(gameObject);
            //TODO: get killed
        }
        return false;
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

}
