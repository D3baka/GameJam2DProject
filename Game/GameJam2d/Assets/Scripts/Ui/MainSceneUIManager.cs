using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneUIManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    [Tooltip("Assign the Shop UI")]
    [SerializeField] GameObject shopScreen;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        shopScreen.GetComponent<Shop>().CloseShop();
    }
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += Instance_OnGameStateChanged;
    }

    private void Instance_OnGameStateChanged(object sender, GameManager.OnGameStateChangedEventArgs e)
    {
        if(e.gameState == GameManager.GameState.GameOver)
        {
            gameOverScreen.SetActive(true);
        }
        
    }
    
    private void OnDestroy()
    {
    }

   
}
 