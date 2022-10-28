using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCast : MonoBehaviour
{
    [SerializeField] private GameObject[] bloodPrefab;
    [SerializeField] private int numOfBloodSpots = 25;
    [SerializeField] private float distance = 15f;
    [SerializeField] private float angle = 85f;
    [SerializeField] private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        Splat();
    }

    void Splat(){
        for(int i = 0; i < 25; i++){
            RaycastHit hit;
            if(Physics.Raycast(transform.position, RandomDirection(), out hit, distance, layerMask)){
                Instantiate(bloodPrefab[Random.Range(0, bloodPrefab.Length)], hit.point + (Vector3.up * 0.01f), Quaternion.Euler(bloodPrefab[0].transform.rotation.eulerAngles.x, Random.Range(0, 360), bloodPrefab[0].transform.rotation.eulerAngles.z));
            }
        }
        Destroy(gameObject, 4f);
    }

    Vector3 RandomDirection()
    {
        return Quaternion.Euler(Random.Range(-angle, angle), 0, Random.Range(-angle, angle)) * Vector3.down;
    }
}
