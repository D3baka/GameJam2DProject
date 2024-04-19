using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monke : MonoBehaviour
{
    private List<int> cards = new List<int>(4);
    
    
    void Start()
    {
        cards.Add((int)Card.Type.LEFT);
        cards.Add((int)Card.Type.SHOOT);
        cards.Add((int)Card.Type.LEFT);
        cards.Add((int)Card.Type.RIGHT);
    }

    public void addCard(Card.Type card)
    {
        cards.Add((int)card);
    }

    public void removeCard(Card.Type card)
    {
        cards.Remove((int)card);
    }

    public Card.Type drawCard()
    {
        // random card
        int index = Random.Range(0, cards.Count);
        Card.Type card = (Card.Type)cards[index];

        return card;
    }
}
