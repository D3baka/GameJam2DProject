using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used exclusively for the main menu - for everywhere else use <c>CreditsUI</c>
/// </summary>
public class MainMenuCreditsUI : MonoBehaviour
{
    [SerializeField] private MainMenuUiController mainMenuUiController;
    [SerializeField] private Button returnButton;

    private void Awake() {
        returnButton.onClick.AddListener(ReturnButtonOnClick);
    }

    private void ReturnButtonOnClick() {
        mainMenuUiController.ToMainMenu();
    }
}
