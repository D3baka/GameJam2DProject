using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Gridmanager gridmanager;


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
}
