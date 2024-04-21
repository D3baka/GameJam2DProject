using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuTutorialUI : MonoBehaviour
{
    [SerializeField] private MainMenuUiController mainMenuUiController;
    [SerializeField] private Button returnButton;

    private void Awake()
    {
        returnButton.onClick.AddListener(ReturnButtonOnClick);
    }

    private void ReturnButtonOnClick()
    {
        AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.buttonClick);
        mainMenuUiController.ToMainMenu();
    }
}
