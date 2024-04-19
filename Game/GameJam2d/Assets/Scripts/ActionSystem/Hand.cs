using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    [SerializeField] private HandCard[] cards;
    private Monke[] monkes;

    void Start()
    {
        cards = new HandCard[4];
        monkes = new Monke[4];

        for (int i = 0; i < monkes.Length; i++)
        {
            monkes[i] = new Monke();
        }
    }


    public void playCard(int index)
    {
        Debug.Log(index);
    }

}
