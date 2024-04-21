using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    
    [SerializeField] private Sprite[] idleSprites;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float delaySeconds;
    [SerializeField] private float delaySecondsMin;
    [SerializeField] private float delaySecondsMax;
    [SerializeField] private bool randomize;


    [SerializeField] private Sprite[] extraAnimation;
    [SerializeField] private Sprite[] extraAnimation2;


    private bool locked = false;
    private float Timer;
    private int zustand = 0;

    private bool playExtraAnimation = false;
    private bool playExtraAnimation2 = false;
    private Sprite[] currentAnimation;

    void Start()
    {
        Timer = 0;
        reDelay();

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (locked) return;
        if (Timer <= Time.time)
        {

            // Switch between idle and extra animation
            if (playExtraAnimation)
                currentAnimation = extraAnimation;
            else if (playExtraAnimation2)
                currentAnimation = extraAnimation2;
            else
                currentAnimation = idleSprites;


            // Add delay for next animation
            if (randomize) reDelay();
            Timer = Time.time + delaySeconds;


            if (zustand >= currentAnimation.Length) zustand = 0;

            if (currentAnimation.Length > 0)
                spriteRenderer.sprite = currentAnimation[zustand];
            zustand = zustand + 1;

        }

        if (UserInput.Instance.GetMovevementVectorNormalized() != Vector2.zero)
        {
            playExtraAnimation2 = true;
        }
        else
        {
            playExtraAnimation2 = false;
        }

    }

    private void reDelay()
    {
        if (!randomize) return;
        delaySeconds = Random.Range(delaySecondsMin, delaySecondsMax);
    }

    public void setLocked()
    {
        locked = true;
    }

    public void playAnimation()
    {
        playExtraAnimation = true;
    }

    public void playAnimation2()
    {
        playExtraAnimation2 = true;
    }

    public void stopAnimation()
    {
        playExtraAnimation = false;
    }

    public void stopAnimation2()
    {
        playExtraAnimation2 = false;
    }
}