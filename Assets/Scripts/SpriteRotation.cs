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
        var lookPos = playerPos.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
    }
}
