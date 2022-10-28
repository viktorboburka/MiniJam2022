using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Reflection;

public class MenuLogic : MonoBehaviour
{


    public void PlayButton()
    {
        MethodBase methodBase = MethodBase.GetCurrentMethod();
        Debug.Log(methodBase.Name); //prints "Start"
    }

    public void CreditsButton()
    {
        MethodBase methodBase = MethodBase.GetCurrentMethod();
        Debug.Log(methodBase.Name); //prints "Start"
    }
    public void QuitButton()
    {
        Application.Quit();

        #if DEBUG
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }


    #region Options
    [SerializeField] private GameObject optionsPanel;
    public void OpenOptionsButton()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptionsButton()
    {
        optionsPanel.SetActive(false);
    }


    #endregion


}
