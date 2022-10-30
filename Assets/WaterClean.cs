using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterClean : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;

    void OnTriggerEnter(Collider coll){
        if(coll.tag == "Blood"){
            Destroy(coll.gameObject);
        }
    }

    private void Start()
    {
        AudioSource.PlayClipAtPoint(soundEffect, GameObject.FindWithTag("Player").transform.position);
    }


    public void DestroyItself() => Destroy(gameObject);
}
