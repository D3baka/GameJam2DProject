using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    private int[] cards;
    private Monke[] monkes;

    void Start()
    {
        cards = new int[4];
        monkes = new Monke[4];

        for (int i = 0; i < monkes.Length; i++)
        {
            monkes[i] = new Monke();
        }
    }


}
