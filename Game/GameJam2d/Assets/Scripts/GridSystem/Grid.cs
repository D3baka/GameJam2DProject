using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debaka.Utils;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Tile[,] tiles;
    Vector3 originPosition;

    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellsize, Vector3 offset)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellsize;
        this.originPosition = offset;

        tiles = new Tile[width, height];
        //construct Tile
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(x, y);
            }
        }

        //Debug Gizmos

        bool isDebug = true;
        if (isDebug)
        {
            debugTextArray = new TextMesh[width, height];
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(tiles[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, new Vector3(0, 0, 1), 20, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);


        }
    }

    public bool GetTileBlockerFromPosition(int _x, int _y, out ITileblocker tileblocker)
    {
        return tiles[_x, _y].GetTileBlocker(out tileblocker);
    }

    public bool IsPositionBlocked(int _x, int _y)
    {
        return tiles[_x, _y].IsBlocked;
    }

    public bool SetTileblockerAtPosition(int _x, int _y, ITileblocker newTileblocker, out ITileblocker _tileblocker)
    {        
        return tiles[_x, _y].Set(out _tileblocker, newTileblocker);
    }

    public bool FreePosition(int _x, int _y, out ITileblocker tileblocker)
    {
        return tiles[_x, _y].Clear(out tileblocker);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

}
