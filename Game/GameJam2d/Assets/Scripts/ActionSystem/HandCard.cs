using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : MonoBehaviour, IInteractable
{

    [SerializeField] public Hand hand;
    [SerializeField] public int index;
    private SpriteRenderer spriteRenderer;
    private SpriteAnimator spriteAnimator;

    [SerializeField] private float playCardAnimationTime = 1.5f;
    private float timer;

    private Card.Type type;

    void Update()
    {
        if (timer <= 0)
        {
            spriteAnimator.stopAnimation();
            return;
        }

        timer -= Time.deltaTime;

       
        spriteAnimator.playAnimation();

    }

    // Start is called before the first frame update
    void Start()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();
    }

    public void Clicked()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.Running){
          if (timer > 0)
            return;
        
            hand.playCard(index);
        }
    }

    public void setType(Card.Type type)
    {
        this.type = type;

        Debug.Log("Set type");

        timer = playCardAnimationTime;

        spriteAnimator.playAnimation();
        spriteAnimator.setIdleSprite(CardIconManager.Instance.GetCardIcon(type));
    }

    public Card.Type getType()
    {
        return this.type;
    }
}
