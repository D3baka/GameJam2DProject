using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monke : MonoBehaviour, IInteractable, IStash
{
    private List<int> cards = new List<int>(4);

    [SerializeField] int evilProb = 5;
    [SerializeField] Hand parentHand;
    [SerializeField] int monkeIndex;

    private SpriteAnimator spriteAnimator;
    private bool evil;
    private float timer;
    [SerializeField] private float drawAnimationTime;

    void Start()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();
    }
    
    void Update()
    {
       
        if (timer <= 0)
        {
            spriteAnimator.stopAnimation();
            spriteAnimator.stopAnimation2();

            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            return;
        }

        timer -= Time.deltaTime;

        // increase size
        
        transform.localScale = new Vector3(1.5f, 1.5f, 1.2f);
       

        if (evil)
            spriteAnimator.playAnimation2();
        else
            spriteAnimator.playAnimation();

    }

    public void addCard(Card.Type card)
    {
        cards.Add((int)card);
        if(cards.Count == 3) { 
            parentHand.monkeReady(monkeIndex);
        }
    }

    public void removeCard(Card.Type card)
    {
        Debug.Log("Das hier wird entfernt: "+ card);
        cards.Remove((int)card);
    }

    public Card.Type drawCard()
    {

        timer = drawAnimationTime;
        evil = false;
        
        if (cards.Count != 3)
        {
            return Card.Type.BLANK;
        }


        // The monke has a chance to randomly give back an evil card.
        int evilRand = Random.Range(0, evilProb);
        if (evilRand == 2)
        {
            evil = true;

            Card.Type[] evilTypes = new Card.Type[2];
            evilTypes[0] = Card.Type.DO_RANDOM_MOVE;
            evilTypes[1] = Card.Type.SPAWN_ASTEROID;
            int i = Random.Range(0, 2);
            return evilTypes[i];
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
