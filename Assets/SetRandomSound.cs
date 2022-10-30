using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] randomSounds;
    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        var randomSound = randomSounds[Random.Range(0, randomSounds.Length)];
        audioSource.clip = randomSound;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
