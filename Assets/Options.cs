using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private AudioMixer masterSFXMixer;
    [SerializeField] private AudioMixer masterMusicMixer;

    [SerializeField] private Slider masterSFX;
    [SerializeField] private Slider masterMusic;
    // Start is called before the first frame update
    void Start()
    {
        masterSFX.value = PlayerPrefs.GetFloat("SFXVolume");
        masterMusic.value = PlayerPrefs.GetFloat("MusicVolume");
        masterSFXMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
        masterMusicMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
    }

    public void SetSFX(float value){
        masterSFXMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    public void SetMusic(float value){
        masterMusicMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }
}
