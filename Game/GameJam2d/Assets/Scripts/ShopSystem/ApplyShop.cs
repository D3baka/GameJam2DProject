using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyShop : MonoBehaviour, IInteractable
{

    [SerializeField] GameObject cardsMenu;


    public void Clicked()
    {
       
        cardsMenu.SetActive(false);
        
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
