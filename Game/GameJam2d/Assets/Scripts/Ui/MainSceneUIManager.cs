using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUIManager : MonoBehaviour
{    
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseMenu;
    

    private void Awake()
    {
        gameOverScreen.SetActive(false);
    }
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += Instance_OnGameStateChanged;        
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= Instance_OnGameStateChanged;
    }

    private void Instance_OnGameStateChanged(object sender, GameManager.OnGameStateChangedEventArgs e)
    {
        if(e.gameState == GameManager.GameState.GameOver)
        {
            gameOverScreen.SetActive(true);
        }
        else if(e.gameState == GameManager.GameState.Paused)
        {
            pauseMenu.SetActive(true);
        }
        else if (e.gameState == GameManager.GameState.Running)
        {
            pauseMenu.SetActive(false);
        }

    }

    

   
}
 