using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    public Transform playerPos;

    void Start(){
        playerPos = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookRot = (new Vector3(playerPos.position.x, 0f, playerPos.position.z) - new Vector3(transform.position.x, 0f, transform.position.z));

        transform.rotation = Quaternion.LookRotation(new Vector3(lookRot.x, transform.position.y, lookRot.z) , Vector3.up);
    }
}
