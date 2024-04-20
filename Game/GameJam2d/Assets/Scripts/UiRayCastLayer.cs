using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiRayCastLayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UserInput.Instance.OnInteractAction += Instance_OnInteractAction;
    }

    private void Instance_OnInteractAction(object sender, System.EventArgs e)
    {
        
        // get coordinates of the mouse click
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            // check if the object clicked implements the interface Interactable
            if (hit.collider.gameObject.GetComponent<IInteractable>() != null)
                hit.collider.gameObject.GetComponent<IInteractable>().Clicked();
        }
    }

   
}
