using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCast : MonoBehaviour
{
    [SerializeField] private GameObject[] bloodPrefab;
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        Destroy(gameObject, 4f);
    }
    
    void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("WorldFloor"))
            return;

        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        int i = 0;

        while (i < numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;
            Instantiate(bloodPrefab[Random.Range(0, bloodPrefab.Length)], collisionEvents[i].intersection + (Vector3.up * 0.01f), Quaternion.FromToRotation(Vector3.forward, collisionEvents[i].normal));
            i++;
        }
    }
}
