using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplat : MonoBehaviour
{
    public float maxScale = 1.2f;
    public float minScale = 0.7f;
    public int damage = 2;
    public float speedMult = 2f;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= Random.Range(minScale, maxScale);
        anim = GetComponent<Animator>();
        anim.speed = 1 + damage * speedMult;
    }
}
