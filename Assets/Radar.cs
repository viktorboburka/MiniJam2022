using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    private List<Collider> enemies = new List<Collider>();
    public List<Collider> GetColliders () { return enemies; }

    public Collider closestEnemy;
    public float lastDistance = 1000f;
    
    void OnTriggerStay(Collider other){
        if(Vector3.Distance(transform.position, other.transform.position) < lastDistance || closestEnemy == null){
            closestEnemy = other;
            lastDistance = Vector3.Distance(transform.position, other.transform.position);
        }
        if(!enemies.Contains(other)){
            enemies.Add(other);
        }
    }

    private void OnTriggerExit (Collider other) {
        if(enemies.Contains(other)){
            enemies.Remove(other);
        }
    }
}
