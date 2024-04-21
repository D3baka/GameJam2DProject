using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsUI : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private Button interactButton;
    [SerializeField] private TextMeshProUGUI interactButtonText;    
    [SerializeField] private Button resetToDefaultsButton;
    [SerializeField] private Button returnButton;

    [SerializeField] private Transform pressToRebindUi;

    private void Awake() {
        returnButton.onClick.AddListener(ReturnButtonOnClick);
        pauseButton.onClick.AddListener(() => { RebindBinding(UserInput.Binding.Pause); });        
        interactButton.onClick.AddListener(() => { RebindBinding(UserInput.Binding.Interact); });
        resetToDefaultsButton.onClick.AddListener(ResetToDefaultsButtonOnClick);
    }

    private void Start() {
        UpdateVisual();
        HidePressToRebindUi();
    }

    private void ReturnButtonOnClick() {
        UiManager.Instance.ExitControls();
    }

    private void UpdateVisual() {
        pauseButtonText.text = UserInput.Instance.GetBindingText(UserInput.Binding.Pause);
        interactButtonText.text = UserInput.Instance.GetBindingText(UserInput.Binding.Interact);
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
        UpdateVisual();
    }
}
