using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIManager : MonoBehaviour
{
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button returnButton;
        
    [SerializeField] private GameObject controlsUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] public GameObject pauseUI;
    [SerializeField] private MainSceneUIManager uIManager;

    List<GameObject> uiList;

    private void Awake()
    {        
        controlsButton.onClick.AddListener(ControlsButtonOnClick);
        settingsButton.onClick.AddListener(SettingsButtonOnClick);
        mainMenuButton.onClick.AddListener(MainMenuButtonOnClick);
        returnButton.onClick.AddListener(ReturnButtonOnClick);

        uiList = new List<GameObject>();
        uiList.Add(controlsUI);
        uiList.Add(settingsUI);
        uiList.Add(pauseUI);
    }

    private void ControlsButtonOnClick()
    {
        EnterSubMenu(controlsUI);
    }

    private void SettingsButtonOnClick()
    {
        EnterSubMenu(settingsUI);
    }

    private void MainMenuButtonOnClick()
    {
        //Load Main scene
    }

    private void ReturnButtonOnClick()
    {
        EnterSubMenu(null);
    }
    

    public void EnterSubMenu(GameObject menu)
    {
        foreach (GameObject uiObject in uiList)
        {
            if (uiObject == menu)
            {
                uiObject.SetActive(true);
            }
            else
            {
                uiObject.SetActive(false);
            }
        }
    }
}
