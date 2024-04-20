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

    [SerializeField] float z = -6;

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

                Vector3 pos = transform.localPosition + new Vector3(j * spacing, -i * spacing, z);

                if (i == 0)
                {
                    if(j == 0)
                    {
                        InstantiateTile(i,j,cornerTile,pos,Quaternion.identity);
                    }
                    else if(j == grid.GetLength(1)-1)
                    {
                        InstantiateTile(i, j, cornerTile, pos, Quaternion.Euler(new Vector3(0, 0, -90)));
                    }
                    else
                    {
                        InstantiateTile(i,j, edgeTile, pos, Quaternion.identity) ;
                    }
                } else if (i ==  grid.GetLength(0)-1){
                    if (j == 0)
                    {
                        InstantiateTile(i, j, cornerTile, pos, Quaternion.Euler(new Vector3(0, 0, 90)));
                    }
                    else if (j == grid.GetLength(1)-1)
                    {
                        InstantiateTile(i, j, cornerTile, pos, Quaternion.Euler(new Vector3(0, 0, 180)));
                    }
                    else
                    {
                        InstantiateTile(i, j, edgeTile, pos, Quaternion.Euler(new Vector3(0, 0, 180)));
                    }
                }
                else
                {
                    if(j == 0)
                    {
                        InstantiateTile(i, j, edgeTile, pos, Quaternion.Euler(new Vector3(0, 0, 90)));
                    }
                    else if(j == grid.GetLength(1) - 1)
                    {
                        InstantiateTile(i, j, edgeTile, pos, Quaternion.Euler(new Vector3(0, 0, -90)));
                    }
                    else
                    {
                        AddEmptyTile(j,i);
                    }
                } 
            }
        }
    }

    private void InstantiateTile(int i, int j, GameObject prefab, Vector3 pos, Quaternion rot)
    {
        grid[i, j] = Instantiate(prefab, pos, rot);
        grid[i, j].transform.parent = transform;
    }

    public void AddEmptyTile(int x, int y)
    {
        Vector3 pos = transform.localPosition + new Vector3(x * spacing, -y * spacing, z);

        InstantiateTile(y, x ,cardTilePrefab, pos, Quaternion.identity);

        grid[y, x].GetComponent<CardTile>().setGridPos(x, y);
        grid[y, x].GetComponent<CardTile>().setGrid(this);
        grid[y, x].GetComponent<CardTile>().setStash(stash);
        grid[y, x].GetComponent<CardTile>().setStackable(stacked);
        
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
