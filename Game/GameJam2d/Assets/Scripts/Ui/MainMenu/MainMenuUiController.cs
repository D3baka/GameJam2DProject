using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Used exclusively for the main menu - for everywhere else use <c>UiManager</c>
/// </summary>
public class MainMenuUiController : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject controlsUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject creditsUI;

    List<GameObject> uiList;
    private void Awake() {
        startGameButton.onClick.AddListener(StartGameButtonOnClick);
        controlsButton.onClick.AddListener(ControlsButtonOnClick);
        settingsButton.onClick.AddListener(SettingsButtonOnClick);
        creditsButton.onClick.AddListener(CreditsButtonOnClick);
        quitButton.onClick.AddListener(QuitButtonOnClick);
        
        uiList = new List<GameObject>();
        uiList.Add(mainMenuUI);
        uiList.Add(controlsUI);
        uiList.Add(settingsUI);
        uiList.Add(creditsUI);
    }

    private void ControlsButtonOnClick() {
        EnterSubMenu(controlsUI);
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
    }

    private void SettingsButtonOnClick() {
        EnterSubMenu(settingsUI);
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
    }
    private void CreditsButtonOnClick() {
        EnterSubMenu(creditsUI);
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
    }
    private void QuitButtonOnClick() {
        Debug.Log("Ending the game!");
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
        Application.Quit();
    }

    private void StartGameButtonOnClick() {
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
        Debug.Log("Starting a new game!");
        Loader.Load(Loader.Scene.GameScene);
    }

    public void ToMainMenu() {
        EnterSubMenu(mainMenuUI);
    }

    private void EnterSubMenu(GameObject menu) {
        foreach(GameObject uiObject in uiList) {
            if(uiObject == menu) {
                uiObject.SetActive(true);
            }
            else {
                uiObject.SetActive(false);
            }
        }
    }
}
