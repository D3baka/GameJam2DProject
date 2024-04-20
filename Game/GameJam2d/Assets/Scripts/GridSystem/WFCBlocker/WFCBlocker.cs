using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCBlocker : MonoBehaviour, ITileblocker
{

    public int xPosition { get; private set; }
    public int yPosition { get; private set; }

    private List<int> possibleState;
    private int state = -1;

    private Dictionary<int, HashSet<int>[]> wfcConstraints;
    
    public void init(int combinations, Dictionary<int, HashSet<int>[]> constraints )
    {
        possibleState = new List<int>();

        for (int i = 0; i < combinations; i++)
        {
            possibleState.Add(i);
            Debug.Log("State:" + i);
        }

        Debug.Log("init states:" + possibleState.Count);

        wfcConstraints = constraints;
    }

    public void SetState(int state)
    {
        this.state = state;
    }

    public List<int> GetPossibleStates()
    {
        return possibleState;
    }

    public int collapse()
    {
        this.state = possibleState[Random.Range(0, possibleState.Count)];

        possibleState.Clear();
        possibleState.Add(state);

        Debug.Log("State:" + state);
        
        return state;
    }

    public int GetPossibleStatesCount()
    {
        return possibleState.Count;
    }

    public int GetState()
    {
        return state;
    }

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
        xPosition = (int)position.x;
        yPosition = (int)position.y;

    }

    public void print()
    {
        Debug.Log(possibleState);
    }

    public void UpdatePossibleStatesBasedOnNeighbor(WFCBlocker neighbor)
    {

    }
}
