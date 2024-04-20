using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    [SerializeField] private HandCard[] cards;
    
    [SerializeField] private Monke[] monkes;

    void Start()
    {
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].setType(monkes[i].drawCard());
        }
    }


    public void playCard(int index)
    {

        Card.Type type = cards[index].getType();


        GameManager.Instance.PlayCard(type);
        GameManager.Instance.NextTurn();

        cards[index].setType(monkes[index].drawCard());
    }

}
