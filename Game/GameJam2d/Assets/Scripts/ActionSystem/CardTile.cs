using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardTile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] public Card.Type type = Card.Type.BLANK;
    [SerializeField] public bool stackable = false;


    private int amount = 0;

    private Vector3 oldPos;

    private int gridX;
    private int gridY;
    private CardGrid grid;

    private IStash stash;


    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void updateIcon()
    {
        spriteRenderer.sprite = CardIconManager.Instance.GetCardIcon(this.type);
    }

    public Card.Type getType()
    {
        return this.type;
    }


    public void OnMouseDrag()
    {
        transform.position = GetMousePos();
    }

    private void OnMouseDown()
    {
        oldPos = transform.position;
    }

    public void OnMouseUp()
    {
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = LayerIgnoreRaycast;
        // get coordinates of the mouse click
        Vector3 mousePos = GetMousePos();
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        int LayerDefault = LayerMask.NameToLayer("Default");
        gameObject.layer = LayerDefault;

        if (hit.collider != null)
        {
            // check if the object clicked implements the interface Interactable
            if (hit.collider.gameObject.GetComponent<CardTile>() != null)
            {
                if (hit.collider.gameObject.GetComponent<CardTile>().setTile(type))
                {
                    amount--;
                    if(amount <= 0)
                    {
                        setTile(Card.Type.BLANK);
                    }
                }
            }
        }

        transform.position = oldPos;
    }

    private Vector3 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    public bool setTile(Card.Type t)
    {
        if(!stackable)
        {
            if(type != Card.Type.BLANK)
            {
                if(t != Card.Type.BLANK)
                {
                    return false;
                }
            }
        }
        else
        {
            t = type;
            incrementAmount();
        }

        stash.changeCard(type, t);
        
        incrementAmount();
        this.type = t;
        updateIcon();
        return true;
    }

    public void setGridPos(int x, int y)
    {
        gridX = x;
        gridY = y; 
    }

    public void setGrid(CardGrid grid)
    {
        this.grid = grid;
    }

    public void setStash(IStash stash)
    {
        this.stash = stash;
    }

    public void setStackable(bool stackable)
    {
        this.stackable = stackable;
    }

    public void incrementAmount()
    {
        amount++;
    }

}
