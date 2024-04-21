using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private GameObject heartIcon;

    [SerializeField] private GameObject shop;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shop.activeSelf)
        {
            tmp.text = "";
            heartIcon.SetActive(false);
        }
        else
        {
            tmp.text = "" + GameManager.Instance.getLives();
            heartIcon.SetActive(true);
        }
    }
}
