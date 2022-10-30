using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text timeT;
    [SerializeField] private TMP_Text enemiesT;
    
    [SerializeField] private WaveManager wave;

    void Start(){
        wave = GameObject.FindWithTag("WaveManager").GetComponent<WaveManager>();
    }

    void Update(){
        timeT.text = "Time Survived: " + StopTime.GetTime<string>();
        enemiesT.text = "Enemies Killed: " + wave.enemiesKilled.ToString();
    }
}
