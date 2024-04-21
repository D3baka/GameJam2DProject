using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used exclusively for the main menu - for everywhere else use <c>ControlsUI</c>
/// </summary>
public class MainMenuControlsUI : MonoBehaviour
{
    
    [SerializeField] private MainMenuUiController mainMenuUiController;

    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private Button resetToDefaultsButton;
    [SerializeField] private Button returnButton;

    [SerializeField] private Transform pressToRebindUi;

    private void Awake() {
        returnButton.onClick.AddListener(ReturnButtonOnClick);
        pauseButton.onClick.AddListener(() => { 
            RebindBinding(UserInput.Binding.Pause);
            AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
        });
        resetToDefaultsButton.onClick.AddListener(ResetToDefaultsButtonOnClick);
    }

    private void Start() {
        UpdateVisual();
        HidePressToRebindUi();
    }

    private void ReturnButtonOnClick() {
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
        mainMenuUiController.ToMainMenu();
    }

    private void UpdateVisual() {
        pauseButtonText.text = UserInput.Instance.GetBindingText(UserInput.Binding.Pause);
    }

    private void ShowPressToRebindUi() {
        pressToRebindUi.gameObject.SetActive(true);
    }

    private void HidePressToRebindUi() {
        pressToRebindUi.gameObject?.SetActive(false);
    }

    private void RebindBinding(UserInput.Binding binding) {
        ShowPressToRebindUi();
        UserInput.Instance.RebindBinding(binding, () => {
            HidePressToRebindUi();
            UpdateVisual();
        });
    }

    private void ResetToDefaultsButtonOnClick() {
        UserInput.Instance.ResetAllBindings();
        UpdateVisual ();
    }
}
