using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGrid : MonoBehaviour
{

    [SerializeField] int height;
    [SerializeField] int width;

    [SerializeField] bool stacked;

    [SerializeField] float spacing = 1.25f;

    [SerializeField] GameObject stashGO;

    private IStash stash;

    [SerializeField] GameObject cardTilePrefab;
    [SerializeField] GameObject cornerTile;
    [SerializeField] GameObject edgeTile;


    private GameObject[,] grid;
    private int cardAmount = 0;


    // Start is called before the first frame update
    void Awake()
    {
        stash = stashGO.GetComponent<IStash>();
        grid = new GameObject[height+2, width+2];

        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++)
            {

                Vector3 pos = transform.localPosition + new Vector3(j * spacing, -i * spacing, 0);

                if (i == 0)
                {
                    if(j == 0)
                    {
                        grid[i, j] = Instantiate(cornerTile, pos, Quaternion.identity);
                    }
                    else if(j == grid.GetLength(1)-1)
                    {
                        grid[i, j] = Instantiate(cornerTile, pos, Quaternion.Euler(new Vector3(0, 0, -90)));
                    }
                    else
                    {
                        grid[i, j] = Instantiate(edgeTile, pos, Quaternion.identity);
                    }
                } else if (i ==  grid.GetLength(0)-1){
                    if (j == 0)
                    {
                        grid[i, j] = Instantiate(cornerTile, pos, Quaternion.Euler(new Vector3(0, 0, 90)));
                    }
                    else if (j == grid.GetLength(1)-1)
                    {
                        grid[i, j] = Instantiate(cornerTile, pos, Quaternion.Euler(new Vector3(0, 0, 180)));
                    }
                    else
                    {
                        grid[i, j] = Instantiate(edgeTile, pos, Quaternion.Euler(new Vector3(0, 0, 180)));
                    }
                }
                else
                {
                    if(j == 0)
                    {
                        grid[i, j] = Instantiate(edgeTile, pos, Quaternion.Euler(new Vector3(0, 0, 90)));
                    }
                    else if(j == grid.GetLength(1) - 1)
                    {
                        grid[i, j] = Instantiate(edgeTile, pos, Quaternion.Euler(new Vector3(0, 0, -90)));
                    }
                    else
                    {
                        AddEmptyTile(j,i);
                    }
                } 
            }
        }
    }

    public void AddEmptyTile(int x, int y)
    {
        Vector3 pos = transform.localPosition + new Vector3(x * spacing, -y * spacing, 0);
        GameObject tile = Instantiate(cardTilePrefab, pos, Quaternion.identity);

        tile.GetComponent<CardTile>().setGridPos(x, y);
        tile.GetComponent<CardTile>().setGrid(this);
        tile.GetComponent<CardTile>().setStash(stash);
        tile.GetComponent<CardTile>().setStackable(stacked);

        grid[y, x] = tile;
    }

    public void AddCard(Card.Type type)
    {
        
        if(cardAmount < width * height)
        {
            if (!stacked)
            {
                getCard(cardAmount).setTile(type);
            }
            else
            {
                for (int i = 0; i < cardAmount; i++)
                {
                    if(getCardType(i) == type)
                    {
                        getCard(i).incrementAmount();
                        return;
                    }
                }
                getCard(cardAmount).setTile(type);
            }
            cardAmount++;
        }
    }

    private Card.Type getCardType(int pos)
    {
        return grid[(pos / width) + 1, (pos % width) + 1].GetComponent<CardTile>().getType();
    }

    private CardTile getCard(int pos)
    {
        return grid[(pos / width) + 1, (pos % width) + 1].GetComponent<CardTile>();
    }

}
