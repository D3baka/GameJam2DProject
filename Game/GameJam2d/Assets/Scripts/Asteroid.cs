using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, ITileblocker
{
    public int xPosition {  get; private set; }
    public int yPosition { get; private set;}

    public void SetPosition(int xPosition, int yPosition)
    {
        this.xPosition = xPosition;
        this.yPosition = yPosition;
    }
}
