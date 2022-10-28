using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplat : MonoBehaviour
{
    public float maxScale = 1.2f;
    public float minScale = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= Random.Range(minScale, maxScale);
    }
}
