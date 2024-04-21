using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour {
    public static UiManager Instance { get; private set; }

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject controlsUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject gameOverUI;

    private void Awake() {
        if (UiManager.Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Too many UiManager Instances: " + UiManager.Instance);
        }
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, GameManager.OnGameStateChangedEventArgs e)
    {
        if(e.gameState == GameManager.GameState.Paused)
        {
            Pause();
        }
        if(e.gameState == GameManager.GameState.Running)
        {
            Unpause();
        }
        if (e.gameState == GameManager.GameState.GameOver)
        {
            GameOver();
        }
    }

    public void Pause() 
    {
        pauseUI.SetActive(true);
    }
    public void Unpause() 
    {
        pauseUI.SetActive(false);
        controlsUI.SetActive(false);
        settingsUI.SetActive(false);
        gameOverUI.SetActive(false);
    }

    public void EnterControls() {
        controlsUI.SetActive(true);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
    }

    public void ExitControls() {
        controlsUI.SetActive(false);
        pauseUI.SetActive(true);
        gameOverUI.SetActive(false);
    }

    public void EnterSettings() {
        settingsUI.SetActive(true);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
    }

    public void ExitSettings() {
        settingsUI.SetActive(false);
        pauseUI.SetActive(true);
        gameOverUI.SetActive(false);
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        pauseUI.SetActive(false);
        controlsUI.SetActive(false);
        settingsUI.SetActive(false);
    }
}
