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
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
        GameManager.Instance.ChangeGameState(GameManager.GameState.Running);
    }

    private void ControlsButtonOnClick() {
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
        UiManager.Instance.EnterControls();
    }

    private void SettingsButtonOnClick() {
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
        UiManager.Instance.EnterSettings();
    }

    private void MainMenuButtonOnClick() {
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
        Loader.Load(Loader.Scene.MainMenu);
    }
}
