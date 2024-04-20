using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Primarely manages the shop UI elements
/// </summary>
public class Shop : MonoBehaviour
{
    public void OpenShop()
    {
        gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
    }
}
