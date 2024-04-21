using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCards : MonoBehaviour, IInteractable
{

    [SerializeField] GameObject cardsMenu;

    [SerializeField] Monke m1;
    [SerializeField] Monke m2;
    [SerializeField] Monke m3;

    public void Clicked()
    {
        if(m1.isReady() && m2.isReady() && m3.isReady())
        {
            cardsMenu.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
