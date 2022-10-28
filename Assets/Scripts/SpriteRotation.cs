using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    public Transform playerPos;

    void Start(){
        playerPos = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookRot = (playerPos.position - transform.position);

        transform.rotation = Quaternion.LookRotation(new Vector3(lookRot.x, transform.position.y, lookRot.z) , Vector3.up);
    }
}
