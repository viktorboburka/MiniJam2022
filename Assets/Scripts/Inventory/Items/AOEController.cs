using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEController : MonoBehaviour
{
    [SerializeField] public Item _item;
    [SerializeField] public string AOEName;
    [SerializeField] public bool destructable;
    [SerializeField] public float timeToDestroy;
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private SphereCollider coll;
    [SerializeField] private ParticleSystem[] particles;

    private Inventory inventory;
    private ParticleSystem.ShapeModule[] shapeParticles;

    List<Enemy> enemies = new List<Enemy>();

    void Start(){
        shapeParticles = new ParticleSystem.ShapeModule[particles.Length];

        int i = 0;
        foreach(ParticleSystem particle in particles){
            shapeParticles[i] = particle.GetComponent<ParticleSystem>().shape;
            i++;
        }


        AOEName = _item.itemName;
        destructable = _item.destructable;
        timeToDestroy = _item.timeToDestroy;
        inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        _item = inventory.items.Find(x => x.itemName == AOEName);

        if(destructable)
            Destroy(gameObject, timeToDestroy);
        
        StartCoroutine(DealAOE(_item.cooldownTick, new AttackInfo(_item.damage, _item.knockback)));
    }

    void Update(){
        for(int i = 0; i< shapeParticles.Length; i++)
            shapeParticles[i].radius = _item.radius + (_item.radius * (inventory.GetItemCount(_item) * 0.15f));

        coll.radius = _item.radius + (_item.radius * (inventory.GetItemCount(_item) * 0.15f));
        rend.gameObject.transform.localScale = (new Vector3(_item.radius, _item.radius, _item.radius) * 2) + ((new Vector3(_item.radius, _item.radius, _item.radius) * 2) * (inventory.GetItemCount(_item) * 0.15f));
    }

    void OnTriggerStay(Collider other){
            Debug.Log(other.gameObject);
        if(other.tag == "Enemy" && !enemies.Contains(other.GetComponent<Enemy>())){
            Debug.Log("Enemy in");
            enemies.Add(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit (Collider other) {
        if(enemies.Contains(other.GetComponent<Enemy>())){
            enemies.Remove(other.GetComponent<Enemy>());
        }
    }

    IEnumerator DealAOE(float _cooldown, AttackInfo atkInfo){
        if(enemies.Count > 0)
            foreach(Enemy enemy in enemies){
                if(enemy != null)
                    enemy.getAttacked(atkInfo);
            }

        yield return new WaitForSeconds(_cooldown);

        StartCoroutine(DealAOE(_cooldown, atkInfo));
    }
}
