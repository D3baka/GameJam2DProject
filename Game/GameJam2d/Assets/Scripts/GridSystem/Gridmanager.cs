using Debaka.Utils;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Gridmanager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroid;
    public static Gridmanager Instance { get; private set; }

    private Grid grid;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Too many Gridmanager Instances: " + Gridmanager.Instance);
        }
        Vector3 offset = Camera.main.ScreenToWorldPoint(new Vector3(0,0,10));
        grid = new Grid(11, 11, 15, offset);
    }

    private void Start()
    {
        SpawnTileBlocker(SpaceGridTileBlocker.Asteroid, 0, grid.height - 1);   
    }

    public void NextTurn()
    {
        TurnMovement();
    }

    private void TurnMovement()
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                ITileblocker oldBlocker;
                MoveTileBlocker(x, y, x, y-1, out oldBlocker);

            }
        }
    }

    private void SpawnTileBlocker(SpaceGridTileBlocker toSpawn, int _x, int _y)
    {
        ITileblocker tileblocker;
        switch (toSpawn)
        {
            case SpaceGridTileBlocker.Asteroid:
                tileblocker = SpawnAsteroid();
                break;
            default:
                tileblocker = SpawnAsteroid();
                break;
                
        }
        ITileblocker old;
        grid.SetTileblockerAtPosition(_x, _y, tileblocker, out old);
        tileblocker.GetGameObject().transform.position = grid.GetWorldPosition(_x, _y) + new Vector3(grid.cellSize / 2, grid.cellSize / 2, 0);
    }

    private bool MoveTileBlocker(int oldX, int oldY,int newX, int newY, out ITileblocker atPositionBefore)
    {
        bool result = false;
        ITileblocker tileblocker;
        atPositionBefore = null;
        if (grid.FreePosition(oldX, oldY, out tileblocker))
        {
            if (!IsInGrid(newX, newY))
            {
                Destroy(tileblocker.GetGameObject());
                return result;
            }
            result = grid.SetTileblockerAtPosition(newX, newY, tileblocker, out atPositionBefore);
            tileblocker.SetGridPosition(new Vector2(newX, newY));
            tileblocker.GetGameObject().transform.position = grid.GetWorldPosition(newX, newY) + new Vector3(grid.cellSize / 2, grid.cellSize / 2, 0);
        }
        return result;
    }

    private ITileblocker SpawnAsteroid()
    {
        Asteroid newAsteroid = Instantiate(asteroid);
        return newAsteroid;
    }

    private bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < grid.width && y < grid.height;
    }

    public enum SpaceGridTileBlocker
    {
        Asteroid,

    }
}
