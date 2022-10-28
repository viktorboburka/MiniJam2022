using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private int hp;
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private float movementSpeed = 3.5f;
    [SerializeField]
    private float range;
    [SerializeField]
    private int dmg;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float pursueDistance;

    [SerializeField]
    private float knockbackSpeed = 20.0f;

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
        Debug.Log(this + " getAttacked called");

        hp -= info.dmg;
        gameObject.GetComponent<EnemyAI>().getKnockedBack(info);
        //TODO: knockback
        //TODO: animation and sound feedback
        if (hp <= 0) {
            //TODO: get killed
        }
        return false;
    }


    private void InitVariables() {
        gameObject.GetComponent<NavMeshAgent>().speed = movementSpeed;
        hp = maxHp;
    }

    public float getPursueDistance() {
        return pursueDistance;
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

}
