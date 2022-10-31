using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    public int experiencePoints = 10;

    public float speed = 1f;

    private Transform target;
    private bool canMove = true;

    void Start(){
        target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate(){
        if(!canMove)
            return;

        var step =  speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == "Player"){
            GetComponent<ParticleSystem>().Stop();
            transform.GetChild(0).gameObject.SetActive(false);
            canMove = false;
            coll.SendMessageUpwards("EXPCollected", experiencePoints, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject, 3f);
        }
    }
}
