using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IStash
{

    [SerializeField] Dictionary<Card.Type, int> prices = new Dictionary<Card.Type, int>();


    [SerializeField] CardGrid cardGrid;
    [SerializeField] CardStash cardStash;


    public void addCard(Card.Type type)
    {
        
    }

    public void cardClicked(Card.Type type)
    {
        if(GameManager.Instance.GetCoinAmount() >= prices[type])
        {
            GameManager.Instance.RemoveCoins(prices[type]);
            cardStash.addNewCard(type);
            Debug.Log("buyed");
        }
    }

    public void changeCard(Card.Type type, Card.Type t)
    {
       
    }

    public void removeCard(Card.Type type)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        prices.Add(Card.Type.SHIELD, 1);
        prices.Add(Card.Type.SHOOT, 2);
        prices.Add(Card.Type.SHUFFLE, 0);


        for(int i = 0; i < prices.Keys.Count; i++)
        {
            cardGrid.AddCard(new List<Card.Type>(prices.Keys)[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
