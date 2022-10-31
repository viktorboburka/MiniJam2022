using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public int healPoints = 10;

    public float speed = 1f;

    private Transform target;
    public bool canMove = false;
    private bool healed = false;

    void Start(){
        target = GameObject.FindWithTag("Player").transform;
        ParticleSystem.EmissionModule paEmission = this.GetComponent<ParticleSystem>().emission;
        paEmission.rateOverTime = healPoints;
    }

    void FixedUpdate(){
        if(!canMove)
            return;

        var step =  speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == "Player" && !healed){
            GetComponent<ParticleSystem>().Stop();
            //transform.GetChild(0).gameObject.SetActive(false);
            canMove = false;
            coll.SendMessageUpwards("GetHealed", healPoints, SendMessageOptions.DontRequireReceiver);
            healed = true;
            Destroy(gameObject, 3f);
        }
    }
}
