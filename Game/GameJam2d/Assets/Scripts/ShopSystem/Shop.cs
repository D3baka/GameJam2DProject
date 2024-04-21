using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IStash
{

    [SerializeField] Dictionary<Card.Type, int> prices = new Dictionary<Card.Type, int>();


    [SerializeField] CardGrid cardGrid;
    [SerializeField] CardStash cardStash;


    public bool acceptCard(Card.Type t)
    {
        return true;
    }

    public void addCard(Card.Type type)
    {
        
    }

    public void cardClicked(Card.Type type)
    {
        if(GameManager.Instance.GetCoinAmount() >= prices[type])
        {
            GameManager.Instance.RemoveCoins(prices[type]);
            cardStash.addNewCard(type);
            AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.shopYes);
        }
        else
        {
            AudioFXPlayer.Instance.PlaySound(AudioFXPlayer.SoundEffect.shopNo);
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
        prices.Add(Card.Type.LEFT, 3);
        prices.Add(Card.Type.FORWARD, 3);
        prices.Add(Card.Type.RIGHT, 3);
        prices.Add(Card.Type.SHOOT, 5);
        prices.Add(Card.Type.SHIELD, 10);
        prices.Add(Card.Type.SHOP, 15);
        prices.Add(Card.Type.SHUFFLE, 12);
        prices.Add(Card.Type.CONIFGURE_MONKE, 15);


        for (int i = 0; i < prices.Keys.Count; i++)
        {
            cardGrid.AddCard(new List<Card.Type>(prices.Keys)[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int getPrice(Card.Type type)
    {
        if (!prices.ContainsKey(type)) return -1;
        return prices[type];
    }
}
