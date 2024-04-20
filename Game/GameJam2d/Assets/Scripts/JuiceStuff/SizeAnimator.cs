using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeAnimator : MonoBehaviour
{

    
    [SerializeField] private float delaySeconds;
    [SerializeField] private float delaySecondsMin;
    [SerializeField] private float delaySecondsMax;
    [SerializeField] private bool randomize;

    private bool locked = false;
    private float Timer;
    private bool stretch = false;

    [SerializeField] private Vector2 startSize;
    [SerializeField] private Vector2 endSize;


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

           

            // Add delay for next animation
            if (randomize) reDelay();
            Timer = Time.time + delaySeconds;


            if (stretch)
            {
                transform.localScale = endSize;
            }
            else
            {
                transform.localScale = startSize;
            }

            stretch = !stretch;
        }


    }

    private void reDelay()
    {
        if (!randomize) return;
        delaySeconds = Random.Range(delaySecondsMin, delaySecondsMax);
    }

   
}