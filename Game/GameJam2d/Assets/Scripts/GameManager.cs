using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Gridmanager gridmanager;
        
    [SerializeField] private int playerMaxHitpoints;
    [SerializeField] private MainSceneUIManager mainSceneUIManager;
    private int playerHitpoints;

    public GameState gameState {  get; private set; }  
    
    public class OnGameStateChangedEventArgs : EventArgs
    {
        public GameState gameState;
    }

    public event EventHandler<OnGameStateChangedEventArgs> OnGameStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Too many GameManager Instances: " + Gridmanager.Instance);
        }

        playerHitpoints = playerMaxHitpoints;
        gameState = GameState.Running;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextTurn();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayCard(Card.Type.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayCard(Card.Type.RIGHT);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            PlayCard(Card.Type.SHOOT);
        }

    }
    public void NextTurn()
    {
        gridmanager.NextTurn();
    }

    public void PlayCard(Card.Type card)
    {
        if(card == Card.Type.LEFT || card == Card.Type.RIGHT) 
        {
            gridmanager.MovePlayer(card);
            return;
        }
        if(card == Card.Type.SHOOT)
        {
            gridmanager.PlayerShoot();
            return;
        }
    }

    public void PlayerHitByAsteroid()
    {
        TakeDamage(1);
    }

    private void TakeDamage(int value)
    {
        playerHitpoints -= value;
        if(playerHitpoints <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("You lost the game time: " + Time.timeAsDouble);
        gameState = GameState.GameOver;
        OnGameStateChanged?.Invoke(this, new OnGameStateChangedEventArgs {  gameState = gameState});

    }

    public enum GameState
    {
        Running,
        Paused,
        GameOver,
        MainMenu
    }
}
