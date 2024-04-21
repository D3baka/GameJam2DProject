using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : MonoBehaviour, IInteractable
{

    [SerializeField] public Hand hand;
    [SerializeField] public int index;
    private SpriteRenderer spriteRenderer;

    private Card.Type type;



    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Clicked()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.Running)
            hand.playCard(index);
    }

    public void setType(Card.Type type)
    {
        this.type = type;

        spriteRenderer.sprite = CardIconManager.Instance.GetCardIcon(type);
    }

    public Card.Type getType()
    {
        return this.type;
    }
}
