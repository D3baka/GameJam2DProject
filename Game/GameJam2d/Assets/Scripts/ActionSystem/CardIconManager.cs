using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIconManager : MonoBehaviour
{
    [SerializeField] private Sprite[] cardIcons;
    public static CardIconManager Instance { get; private set; }
    // Update is called once per frame

    public Sprite GetCardIcon(Card.Type type)
    {
        if (cardIcons.Length < (int)type)
        {
            Debug.LogError("Card Icon not found for type: " + type);
            return null;
        }
        
        return cardIcons[(int)type];
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Too many GameManager Instances: " + UserInput.Instance);
        }
    }
    }
