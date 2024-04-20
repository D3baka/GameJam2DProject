using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class WFCBlocker : MonoBehaviour, ITileblocker
{

    public int xPosition { get; private set; }
    public int yPosition { get; private set; }

    private List<int> possibleState;
    private int state = -1;
    private int[] probabilities;

    private Dictionary<int, HashSet<int>[]> wfcConstraints;
    
    public void init(int combinations, Dictionary<int, HashSet<int>[]> constraints, int[] probabilityList )
    {
        possibleState = new List<int>();

        for (int i = 0; i < combinations; i++)
        {
            possibleState.Add(i);
            //Debug.Log("State:" + i);
        }

        //Debug.Log("init states:" + possibleState.Count);

        wfcConstraints = constraints;

        probabilities = probabilityList;
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
        int randomUpperCorner = 0;
        foreach (int element in possibleState)
        {
            randomUpperCorner += probabilities[element];
        }

        int randomNum = Random.Range(0, randomUpperCorner);

        int curr = 0;
        int index = -1;
        foreach(int element in possibleState)
        {
            curr += probabilities[element];
            if (curr >= randomNum)
            {
                index = element;
                break;
            }

        }


        this.state = index;

        possibleState.Clear();
        possibleState.Add(state);

        //Debug.Log("State:" + state);
        
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
        //Debug.Log(possibleState);
    }

    public void UpdatePossibleStatesBasedOnNeighbor(int state, int x, int y)
    {
        int directionIndex;
        if (x == this.xPosition - 1 && y == this.yPosition)
            directionIndex = 0; // Left
        else if (x == this.xPosition + 1 &&y == this.yPosition)
            directionIndex = 1; // Right
        else if (x == this.xPosition && y == this.yPosition - 1)
            directionIndex = 2; // Up
        else if (x == this.xPosition && y == this.yPosition + 1)
            directionIndex = 3; // Down
        else
            return; // Not a direct neighbor

        
        if (wfcConstraints.ContainsKey(state))
        {
            HashSet<int> allowedStates = wfcConstraints[state][directionIndex];
            foreach (int el in allowedStates)
            {
                Debug.Log("Element " + el);
            }

            // Filter the possible states based on the allowed states
            possibleState = possibleState.Where(s => allowedStates.Contains(s)).ToList();


            if (possibleState.Count == 0)
            {
                Debug.LogError("No valid states remain for tile at position (" + xPosition + ", " + yPosition + ")");
                // Handle error state where no valid states are possible
            }
        }
    }

    public void UpdatePossibleStatesBasedOnNeighbor(WFCBlocker neighbor)
    {
        //Debug.Log("=======================");
        //Debug.Log("collapsing cell" + xPosition);
        //Debug.Log("Neighbor state:" + neighbor.GetState());
        // Determine the relative direction of the neighbor
        int directionIndex;
        if (neighbor.xPosition == this.xPosition - 1 && neighbor.yPosition == this.yPosition)
            directionIndex = 0; // Left
        else if (neighbor.xPosition == this.xPosition + 1 && neighbor.yPosition == this.yPosition)
            directionIndex = 1; // Right
        else if (neighbor.xPosition == this.xPosition && neighbor.yPosition == this.yPosition - 1)
            directionIndex = 2; // Up
        else if (neighbor.xPosition == this.xPosition && neighbor.yPosition == this.yPosition + 1)
            directionIndex = 3; // Down
        else
            return; // Not a direct neighbor

        // Get the neighbor's state and retrieve the corresponding allowed states for the current tile
        int neighborState = neighbor.GetState();
        if (wfcConstraints.ContainsKey(neighborState))
        {
            HashSet<int> allowedStates = wfcConstraints[neighborState][directionIndex];
            foreach (int el in allowedStates)
            {
                //Debug.Log("Element " + el);  
            }

            // Filter the possible states based on the allowed states
            possibleState = possibleState.Where(s => allowedStates.Contains(s)).ToList();


            if (possibleState.Count == 0)
            {
                Debug.LogError("No valid states remain for tile at position (" + xPosition + ", " + yPosition + ")");
                // Handle error state where no valid states are possible
            }
        }
    }

}
