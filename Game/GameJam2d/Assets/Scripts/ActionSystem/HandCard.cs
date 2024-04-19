using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : MonoBehaviour, IInteractable
{

    [SerializeField] private Hand hand;
    [SerializeField] private int index;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void Clicked()
    {
       hand.playCard(index);
    }
}
