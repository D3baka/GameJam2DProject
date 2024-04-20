using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    [SerializeField] private HandCard[] cards;
    
    [SerializeField] private Monke[] monkes;

    void Start()
    {
      
    }


    public void playCard(int index)
    {

        Card.Type type = cards[index].getType();

        cards[index].setType(monkes[index].drawCard());
    }

}
