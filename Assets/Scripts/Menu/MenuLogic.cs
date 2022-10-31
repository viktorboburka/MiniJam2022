using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1);//later should be replaced with string
    }

    public void QuitButton()
    {
        Application.Quit();

        #if DEBUG
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }


    public void QuitButtonToMenu()
    {
        SceneManager.LoadScene(0);
    }



    #region Credits

    [SerializeField] private GameObject CreditsPanel;
    public void OpenCreditsButton()
    {
        CreditsPanel.SetActive(true);
    }
    public void CloseCredisButton()
    {
        CreditsPanel.SetActive(false);
    }
    #endregion


    #region Options

    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Toggle fullscreen;
    public void OpenOptionsButton()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptionsButton()
    {
        optionsPanel.SetActive(false);
    }

    public void TrigerFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void ChangeScreenResolution(int setting)
    {
        switch (setting)
        {
            case 0: Screen.SetResolution(3840, 2160, Screen.fullScreen); break;
            case 1: Screen.SetResolution(2560, 1440, Screen.fullScreen); break;
            case 2: Screen.SetResolution(1920, 1080, Screen.fullScreen); break;
            case 3: Screen.SetResolution(1280, 720, Screen.fullScreen); break;
            case 4: Screen.SetResolution(640, 480, Screen.fullScreen); break;
            case 5: Screen.SetResolution(69, 69, Screen.fullScreen); break;
        }

    }


    #endregion


    private void Start()
    {
        fullscreen.isOn = fullscreen;
    }

}
