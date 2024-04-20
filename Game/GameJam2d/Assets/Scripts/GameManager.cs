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
        
    }

    public void NextTurn()
    {
        gridmanager.NextTurn();
    }
}
