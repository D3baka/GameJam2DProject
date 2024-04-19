using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardStash : MonoBehaviour
{

    private List<int> cards;

    public CardStash() {

        cards = new List<int>();
    
        for(int i = 0; i < 4; i++)
        {
            addCard(Card.Type.LEFT);
            addCard(Card.Type.RIGHT);
            addCard(Card.Type.SHOOT);
        }
            
    }

    public void addCard(Card.Type card)
    {
        cards.Add((int)card);
    }

    public void removeCard(Card.Type card)
    {
        cards.Remove((int)card);
    }

}
