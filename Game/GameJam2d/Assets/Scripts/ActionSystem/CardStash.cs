using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CardStash : MonoBehaviour, IStash
{

    private List<int> cards;

    [SerializeField] CardGrid grid;

    [SerializeField] GameObject monkeConfig;


    private List<int> pendingCards = new List<int>();

    void Start() {

        cards = new List<int>();
    
        for(int i = 0; i < 2; i++)
        {
            grid.AddCard(Card.Type.LEFT);
            grid.AddCard(Card.Type.RIGHT);
            grid.AddCard(Card.Type.FORWARD);
            grid.AddCard(Card.Type.SHOOT);
        }
        grid.AddCard(Card.Type.SHOP);
        grid.AddCard(Card.Type.CONIFGURE_MONKE);


    }

    void Update()
    {
        if(pendingCards.Count > 0 && monkeConfig.activeSelf)
        {
            for(int i = 0;i < pendingCards.Count;i++)
            {
                grid.AddCard((Card.Type)pendingCards[i]);
            }
            pendingCards.Clear();
        }
    }

    public void addCard(Card.Type card)
    {
        cards.Add((int)card);
    }

    public void addNewCard(Card.Type card)
    {
        if (!monkeConfig.activeSelf)
        {
            pendingCards.Add((int) card);
        }
        else
        {
            grid.AddCard(card);
        }
    }

    public void removeCard(Card.Type card)
    {
        cards.Remove((int)card);
    }

    public void changeCard(Card.Type type, Card.Type t)
    {
        addCard(t);
    }

    public void cardClicked(Card.Type type)
    {
        throw new System.NotImplementedException();
    }
}
