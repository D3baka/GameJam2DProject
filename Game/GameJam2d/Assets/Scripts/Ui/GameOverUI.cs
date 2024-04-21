using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(MainMenuButtonOnClick);
        
    }

    private void MainMenuButtonOnClick()
    {
        Loader.Load(Loader.Scene.MainMenu);
    }

}
