using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PriceDisplay : MonoBehaviour
{
    [SerializeField] Card.Type type;
    [SerializeField] Shop shop;

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "" + shop.getPrice(type);

    }
}
