using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopIconDisplay : MonoBehaviour
{

    [SerializeField] Card.Type type;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite =  CardIconManager.Instance.GetCardIcon(type);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
