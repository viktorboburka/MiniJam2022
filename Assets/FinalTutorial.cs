using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTutorial : MonoBehaviour
{
    public void Finish(){
        GameObject.FindWithTag("WaveManager").GetComponent<WaveManager>().enabled = true;
        Destroy(gameObject);
        StopTime.resetTimer();
    }
}
