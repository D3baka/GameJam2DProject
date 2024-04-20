using Debaka.Utils;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Gridmanager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private  PlayerShip playerShipPrefab;
    public static Gridmanager Instance { get; private set; }

    private Grid grid;
    private PlayerShip playerShip;

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
        SpawnTileBlocker(SpaceGridTileBlocker.PlayerShip, 5, 0);
        SpawnTileBlocker(SpaceGridTileBlocker.Asteroid, 0, grid.height - 1);
        SpawnTileBlocker(SpaceGridTileBlocker.Asteroid, 5, grid.height - 1);
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
                ITileblocker toMove;
                if(grid.GetTileBlockerFromPosition(x,y, out toMove))
                {
                    if(toMove.GetGameObject().GetComponent<PlayerShip>() != null)
                    {
                        //Do nothing because we do not move the player in turns
                        continue;
                    }
                }
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
            case SpaceGridTileBlocker.PlayerShip:
                tileblocker = SpawnPlayerShip();
                playerShip = tileblocker as PlayerShip;
                break;
            default:
                tileblocker = SpawnAsteroid();
                break;
                
        }
        ITileblocker old;
        grid.SetTileblockerAtPosition(_x, _y, tileblocker, out old);
        tileblocker.SetGridPosition(new Vector2(_x, _y));
        tileblocker.GetGameObject().transform.position = grid.GetWorldPosition(_x, _y) + new Vector3(grid.cellSize / 2, grid.cellSize / 2, 0);
    }

    private bool MoveTileBlocker(int oldX, int oldY,int newX, int newY, out ITileblocker atPositionBefore)
    {
        bool result = false;
        ITileblocker tileblocker;
        atPositionBefore = null;

        // Check if we actually have something to move at the position
        if (grid.FreePosition(oldX, oldY, out tileblocker))
        {
            //check if target position is in grid
            if (!IsInGrid(newX, newY))
            {
                DestroyTileBlocker(tileblocker);
                return result;
            }
            result = grid.SetTileblockerAtPosition(newX, newY, tileblocker, out atPositionBefore);

            //if there is a  Tileblocker at the target position      
            if(result)
            {
                //if either the thing we move or the thing we collide with is the player ship
                if (atPositionBefore.GetGameObject().GetComponent<PlayerShip>() != null)
                {
                    //interact!!!
                    OnShipCollidedWithTileBlocker(tileblocker);
                }
                if(tileblocker.GetGameObject().GetComponent<PlayerShip>() != null)
                {
                    OnShipCollidedWithTileBlocker(atPositionBefore);
                }
            }
            tileblocker.SetGridPosition(new Vector2(newX, newY));
            tileblocker.GetGameObject().transform.position = grid.GetWorldPosition(newX, newY) + new Vector3(grid.cellSize / 2, grid.cellSize / 2, 0);
        }
        return result;
    }

    private ITileblocker SpawnAsteroid()
    {
        Asteroid newAsteroid = Instantiate(asteroidPrefab);
        return newAsteroid;
    }

    private ITileblocker SpawnPlayerShip()
    {
        PlayerShip newPlayerShip = Instantiate(playerShipPrefab);
        return newPlayerShip;
    }

    private bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < grid.width && y < grid.height;
    }

    private void OnShipCollidedWithTileBlocker(ITileblocker collidingBlocker)
    {
        if(collidingBlocker.GetGameObject().GetComponent<Asteroid>() != null)
        {
            Debug.Log("Ship hit Asteroid!!!");
            DestroyTileBlocker(collidingBlocker);
        }        
    }

    private void DestroyTileBlocker(ITileblocker tileblocker)
    {
        Destroy(tileblocker.GetGameObject());
    }

    public void MovePlayer(Card.Type card)
    {
        switch (card)
        {
            case Card.Type.LEFT:
                {
                    Debug.Log("Moving player left from position: " + playerShip.xPosition + " " + playerShip.yPosition);
                    ITileblocker old;
                    int newX = playerShip.xPosition - 1;
                    if (newX < 0)
                    {
                        newX = grid.width - 1;
                    }
                    MoveTileBlocker(playerShip.xPosition, playerShip.yPosition, newX, playerShip.yPosition, out old);
                    break;
                }
                
            case Card.Type.RIGHT:
                {
                    Debug.Log("Moving player right from position: " + playerShip.xPosition + " " + playerShip.yPosition);
                    ITileblocker old;
                    int newX = playerShip.xPosition + 1;
                    if (newX >= grid.width)
                    {
                        newX = 0;
                    }
                    MoveTileBlocker(playerShip.xPosition, playerShip.yPosition, newX, playerShip.yPosition, out old);
                    break;
                }
                
            case Card.Type.SHOOT:
                break;
        }
    }    


    public enum SpaceGridTileBlocker
    {
        Asteroid,
        PlayerShip,

    }
}
