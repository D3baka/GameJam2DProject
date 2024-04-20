using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monke : MonoBehaviour, IInteractable, IStash
{
    private List<int> cards = new List<int>(4);
    
    
    void Start()
    {
    }

    public void addCard(Card.Type card)
    {
        cards.Add((int)card);
    }

    public void removeCard(Card.Type card)
    {
        Debug.Log("Das hier wird entfernt: "+ card);
        cards.Remove((int)card);
    }

    public Card.Type drawCard()
    {

        if (cards.Count != 4)
        {
            return Card.Type.BLANK;
        }

        // random card
        int index = Random.Range(0, cards.Count);
        Card.Type card = (Card.Type)cards[index];

        return card;
    }

    public void Clicked()
    {
        //popup

    }

    public void changeCard(Card.Type oldCard, Card.Type newCard)
    {
        Debug.Log("old card " + oldCard + " to " + newCard);
        removeCard(oldCard);
        if(newCard != Card.Type.BLANK)
        {
            addCard(newCard);
        }
    }
}
