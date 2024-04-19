using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Tile[,] tiles;
    Vector3 originPosition;

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
    }

    bool GetTileBlockerFromPosition(int _x, int _y, out Tileblocker tileblocker)
    {
        return tiles[_x, _y].GetTileBlocker(out tileblocker);
    }

    bool IsPositionBlocked(int _x, int _y)
    {
        return tiles[_x, _y].IsBlocked;
    }

    bool SetTileblockerAtPosition(int _x, int _y, Tileblocker newTileblocker, out Tileblocker _tileblocker)
    {        
        return tiles[_x, _y].Set(out _tileblocker, newTileblocker);
    }

    bool FreePosition(int _x, int _y, out Tileblocker tileblocker)
    {
        return tiles[_x, _y].Clear(out tileblocker);
    }


   
}
