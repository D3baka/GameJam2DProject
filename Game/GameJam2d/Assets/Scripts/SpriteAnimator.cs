using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    
    [SerializeField] private Sprite[] idleSprites;

    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private float delaySeconds;
    [SerializeField] private float delaySecondsMin;
    [SerializeField] private float delaySecondsMax;
    [SerializeField] private bool randomize;


    [SerializeField] private Sprite[] extraAnimation;


    private bool locked = false;
    private float Timer;
    private int zustand = 0;

    private bool playExtraAnimation = false;
    private Sprite[] currentAnimation;

    void Start()
    {
        Timer = 0;
        reDelay();
        
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
            else
                currentAnimation = idleSprites;


            // Add delay for next animation
            if (randomize) reDelay();
            Timer = Time.time + delaySeconds;


            if (zustand >= currentAnimation.Length) zustand = 0;

            renderer.sprite = currentAnimation[zustand];
            zustand = zustand + 1;


           
        }

        if (UserInput.Instance.GetMovevementVectorNormalized() != Vector2.zero)
        {
            playExtraAnimation = true;
        }
        else
        {
            playExtraAnimation = false;
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

    public void playAction()
    {
        playExtraAnimation = true;
    }

    public void stopAction()
    {
        playExtraAnimation = false;
    }
}