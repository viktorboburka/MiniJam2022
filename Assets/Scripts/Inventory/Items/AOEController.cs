using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEController : MonoBehaviour
{
    public string AOEName;
    public Inventory inventory;
    
    public Item _item;

    List<Enemy> enemies = new List<Enemy>();

    void Start(){
        inventory = transform.parent.GetComponent<Inventory>();
        Debug.Log(inventory.items[0]);
        _item = inventory.items.Find(x => x.itemName == AOEName);
        StartCoroutine(DealAOE(_item.cooldown, new AttackInfo(_item.damage, _item.knockback)));
    }

    void OnTriggerEnter(Collider coll){
        if(coll.tag == "Enemy")
            enemies.Add(coll.gameObject.GetComponent<Enemy>());
    }

    void OnTriggerExit(Collider coll){
        if(coll.tag == "Enemy")
        if(enemies.Contains(coll.gameObject.GetComponent<Enemy>()))
             enemies.Remove(coll.gameObject.GetComponent<Enemy>());
    }

    IEnumerator DealAOE(float _cooldown, AttackInfo atkInfo){
        if(enemies.Count > 0)
            foreach(Enemy enemy in enemies){
                enemy.getAttacked(atkInfo);
            }

        yield return new WaitForSeconds(_cooldown);

        StartCoroutine(DealAOE(_cooldown, atkInfo));
    }
}
