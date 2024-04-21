using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button returnButton;



    private void Awake() {
        returnButton.onClick.AddListener(ReturnButtonOnClick);
        controlsButton.onClick.AddListener(ControlsButtonOnClick);
        settingsButton.onClick.AddListener(SettingsButtonOnClick);

        mainMenuButton.onClick.AddListener(MainMenuButtonOnClick);
    }

    private void ReturnButtonOnClick() {
        GameManager.Instance.ChangeGameState(GameManager.GameState.Running);
    }

    private void ControlsButtonOnClick() {        
        UiManager.Instance.EnterControls();
    }

    private void SettingsButtonOnClick() {
        UiManager.Instance.EnterSettings();
    }

    private void MainMenuButtonOnClick() {
        Loader.Load(Loader.Scene.MainMenu);
    }
}
