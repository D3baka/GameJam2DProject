using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardStash : MonoBehaviour, IStash
{

    private List<int> cards;

    void Start() {

        cards = new List<int>();
    
        for(int i = 0; i < 2; i++)
        {
            GetComponent<CardGrid>().AddCard(Card.Type.LEFT);
            GetComponent<CardGrid>().AddCard(Card.Type.RIGHT);
            GetComponent<CardGrid>().AddCard(Card.Type.FORWARD);
            GetComponent<CardGrid>().AddCard(Card.Type.SHOOT);
        }
        GetComponent<CardGrid>().AddCard(Card.Type.SHOP);

    }

    public void addCard(Card.Type card)
    {
        cards.Add((int)card);
    }

    public void removeCard(Card.Type card)
    {
        cards.Remove((int)card);
    }

    public void changeCard(Card.Type type, Card.Type t)
    {
        addCard(t);
    }
}
