using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour, ITileblocker
{
    public int xPosition { get; private set; }
    public int yPosition { get; private set; }

    [SerializeField] public GameObject shieldVisual;
    public void SetPosition(int xPosition, int yPosition)
    {
        this.xPosition = xPosition;
        this.yPosition = yPosition;
    }
    public Vector2 GetGridPosition()
    {
        return new Vector2(xPosition, yPosition);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SetGridPosition(Vector2 position)
    {
        Debug.Log("Setting playership position to: " +  position);
        xPosition = (int)position.x;
        yPosition = (int)position.y;

    }


}
