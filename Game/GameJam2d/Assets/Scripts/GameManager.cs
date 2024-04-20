using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Gridmanager gridmanager;
        
    [SerializeField] private int playerMaxHitpoints;
    [SerializeField] private MainSceneUIManager mainSceneUIManager;
    private int playerHitpoints;

    [SerializeField] private int coinCount;
    [SerializeField] private GameObject shop;
    private bool isShielded = false;
    private int shieldTimeLeft;


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
        if (isShielded)
        {
            shieldTimeLeft--;
            if(shieldTimeLeft <= 0)
            {
                isShielded = false;
                gridmanager.DeactivateShield();
            }
        }
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
        if(card == Card.Type.SHOP)
        {
            shop.GetComponent<Shop>().OpenShop();
            return;
        }
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
    }

    public void RemoveCoins(int amount)
    {
        if(coinCount - amount <= 0)
        {
            coinCount -= amount;
        }
        else
        {
            Debug.LogError("Cannot remove more coins than player has - use GetCoinAmount before calling this"); 
        }
    }

    public int GetCoinAmount()
    {
        return coinCount;
    }


    public void PlayerHitByAsteroid()
    {
        TakeDamage(1);
    }

    private void TakeDamage(int value)
    {
        if(isShielded)
        {
            isShielded = false;
            gridmanager.DeactivateShield();
            return;
        }
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

    private void GainShield()
    {
        isShielded = true;
        shieldTimeLeft = 5;
        gridmanager.ActivateShield();
    }

    public enum GameState
    {
        Running,
        Paused,
        GameOver,
        MainMenu
    }
}
