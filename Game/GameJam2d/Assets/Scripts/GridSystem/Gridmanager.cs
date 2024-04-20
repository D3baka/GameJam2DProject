using Debaka.Utils;
using System.Collections.Generic;
using UnityEngine;

public class Gridmanager : MonoBehaviour
{

    [SerializeField] private Asteroid asteroid;
    [SerializeField] private AsteroidCore asteroid2;

    [SerializeField] private WFCBlocker wfcBlocker;

    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private  PlayerShip playerShipPrefab;
    [SerializeField] private Projectile projectilePrefab;

    public static Gridmanager Instance { get; private set; }

    // array that represents tile map:
    private int[,] wfcGridArray;
    private int wfcElementCount;
    private int[] probability;

    private Grid grid;
    private PlayerShip playerShip;
    private List<Projectile> projectiles = new();

    private Dictionary<int, HashSet<int>[]> wfcConstraints;

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

        probability = new int[3];
        probability[0] = 100;
        probability[1] = 30;
        probability[2] = 80;

        wfcGridArray = new int[,]
        {
           {0, 0, 0, 0, 0, 0 },
           {0, 0, 1, 0, 0, 0 },
           {0, 1, 2, 1, 0, 0 },
           {0, 0, 1, 0, 0, 0 },
           {0, 0, 0, 0, 0, 0 }
        };

        // Iterate through wfcGridArray to find the highest element
        wfcElementCount = 0;
        for (int i = 0; i < wfcGridArray.GetLength(0); i++)
        {
            for (int j = 0; j < wfcGridArray.GetLength(1); j++)
            {
                if (wfcGridArray[i, j] > wfcElementCount)
                {
                    wfcElementCount = wfcGridArray[i, j];
                }
            }
        }

        wfcElementCount += 1;

        wfcConstraints = new Dictionary<int, HashSet<int>[]>();

        ExtractAdjacencyRules();
        generateWorld();
    }

    private void ExtractAdjacencyRules()
    {
        int rows = wfcGridArray.GetLength(0);
        int cols = wfcGridArray.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int currentTile = wfcGridArray[i, j];
                if (!wfcConstraints.ContainsKey(currentTile))
                {
                    wfcConstraints[currentTile] = new HashSet<int>[4]; // 0: left, 1: right, 2: up, 3: down
                    for (int k = 0; k < 4; k++)
                    {
                        wfcConstraints[currentTile][k] = new HashSet<int>();
                    }
                }

                // Left
                if (j > 0)
                {
                    int leftTile = wfcGridArray[i, j - 1];
                    wfcConstraints[currentTile][0].Add(leftTile);
                }
                // Right
                if (j < cols - 1)
                {
                    int rightTile = wfcGridArray[i, j + 1];
                    wfcConstraints[currentTile][1].Add(rightTile);
                }
                // Up
                if (i > 0)
                {
                    int upTile = wfcGridArray[i - 1, j];
                    wfcConstraints[currentTile][2].Add(upTile);
                }
                // Down
                if (i < rows - 1)
                {
                    int downTile = wfcGridArray[i + 1, j];
                    wfcConstraints[currentTile][3].Add(downTile);
                }
            }
        }
    }

    private void generateWorld()
    {
        // fill grid with wfc Tile Blocker

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                SpawnTileBlocker(SpaceGridTileBlocker.WFCTileBlocker, x, y);

            }
        }

        // debug logs:

        //Debug.Log("spawned");

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                ITileblocker blocker = null;
                grid.GetTileBlockerFromPosition(x, y, out blocker);

                // cast to wfcBlocker
                WFCBlocker wfcBlocker = blocker as WFCBlocker;
                if (wfcBlocker != null)
                {
                    wfcBlocker.print();
                }
            }
        }

        
        // now lets prefill the first three 3 rows (empty)
        int empytRows = 3;

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < empytRows; y++)
            {
                ITileblocker blocker = null;
                grid.GetTileBlockerFromPosition(x, y, out blocker);


                WFCBlocker wfcBlocker = blocker as WFCBlocker;
                
                wfcBlocker.SetState(0);

                updateNeighborsConstraints(wfcBlocker, y);
            }
        }
        for (int y = 0; y < empytRows; y++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                ITileblocker blocker = null;
                grid.FreePosition(x, y, out blocker);

                int state = (blocker as WFCBlocker).GetState();

                Destroy(blocker.GetGameObject());

                switch (state)
                {
                    case 0:
                        // do nothing because nothingness and empytness is in space as well in my thoughts 
                        break;
                    case 1:
                        SpawnTileBlocker(SpaceGridTileBlocker.Asteroid, x, y);
                        break;
                    case 2:
                        SpawnTileBlocker(SpaceGridTileBlocker.AsteroidCenter, x, y);
                        break;
                    default:
                        break;
                }
            }

        }
        // now iterate through remaining rows and do wfc collapse
        for (int y = empytRows; y < grid.height; y++)
        {
            collapseRow(y);
        }
        
    }

    private void collapseRow(int row)
    {
       
        
         while (!collapsed(row))
         {
             WFCBlocker lowestEntropy = getLowestEntropy(row);

             lowestEntropy.collapse();

             updateNeighborsConstraints(lowestEntropy, row);
         }

          // update visuals:

          for(int x = 0; x < grid.width; x++)
          {
              ITileblocker blocker = null;
              grid.FreePosition(x, row, out blocker);

              int state = (blocker as WFCBlocker).GetState();

              Destroy(blocker.GetGameObject());

              switch (state)
              {
                  case 0:
                      // do nothing because nothingness and empytness is in space as well in my thoughts 
                      break;
                  case 1:
                      SpawnTileBlocker(SpaceGridTileBlocker.Asteroid, x, row);
                      break;
                case 2:
                    SpawnTileBlocker(SpaceGridTileBlocker.AsteroidCenter, x, row);
                    break;
                  default:
                      break;
              }
          }
          
    }

    private void updateNeighborsConstraints(WFCBlocker collapsedBlocker, int row)
    {
        //Debug.Log("UPDATE NEIGHBORS");
        int x = collapsedBlocker.xPosition;
        int y = row;

        // Neighboring positions to check: left, right, above, below
        List<Vector2Int> neighbors = new List<Vector2Int>
    {
        new Vector2Int(x - 1, y), // left
        new Vector2Int(x + 1, y), // right
        new Vector2Int(x, y + 1)  // below
    };

        foreach (var neighbor in neighbors)
        {
            //Debug.Log("ITERATING)");
            if (IsInGrid(neighbor.x, neighbor.y))
            {
                //Debug.Log("ITERATING IN GRID" + neighbor.x + ", " + neighbor.y);
                ITileblocker neighborBlocker;
                grid.GetTileBlockerFromPosition(neighbor.x, neighbor.y, out neighborBlocker );
                WFCBlocker wfcNeighbor = neighborBlocker as WFCBlocker;
                //Debug.Log(wfcNeighbor == null);
                if (wfcNeighbor != null)
                {
                    wfcNeighbor.UpdatePossibleStatesBasedOnNeighbor(collapsedBlocker);
                }
            }
        }
    }

    private bool collapsed(int row)
    {
        bool collapsed = true;
        for(int x = 0; x < grid.width; x++)
        {
            ITileblocker blocker = null;
            grid.GetTileBlockerFromPosition(x, row, out blocker);

            // cast to wfcBlocker
            WFCBlocker wfcBlocker = blocker as WFCBlocker;


            collapsed = collapsed && (wfcBlocker.GetState() != -1);
        }
        
        return collapsed;
    }

    private WFCBlocker getLowestEntropy(int row)
    {
        WFCBlocker lowestEntropyBlocker = null;
        int minimumEntropy = int.MaxValue; // Use int.MaxValue for a safe initial high value.
        List<WFCBlocker> candidates = new List<WFCBlocker>();

        for (int x = 0; x < grid.width; x++)
        {
            grid.GetTileBlockerFromPosition(x, row, out ITileblocker tempBlocker);

            // Cast to WFCBlocker
            WFCBlocker wfcBlocker = tempBlocker as WFCBlocker;
            if (wfcBlocker != null)
            {
                if (wfcBlocker.GetState() != -1)
                    continue;

                int currentEntropy = wfcBlocker.GetPossibleStatesCount();
                if (currentEntropy < minimumEntropy)
                {
                    minimumEntropy = currentEntropy;
                    candidates.Clear(); // Clear previous candidates since we found a new lower entropy
                    candidates.Add(wfcBlocker);
                }
                else if (currentEntropy == minimumEntropy)
                {
                    candidates.Add(wfcBlocker);
                }
            }
        }

        // If there are multiple candidates with the same entropy, randomly pick one
        if (candidates.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, candidates.Count);
            lowestEntropyBlocker = candidates[randomIndex];
        }

        return lowestEntropyBlocker;
    }

    private void generateNewLine()
    {
        // fill last line with wfc blocker
        for(int x = 0; x < grid.width; x++)
        {
            SpawnTileBlocker(SpaceGridTileBlocker.WFCTileBlocker, x, grid.height-1);

        }
        bool change = true;
        int y = grid.height - 1;
        while (change)
        {
            change = false;
            for (int x = 0; x < grid.width; x++)
            {
                ITileblocker blocker = null;
                grid.GetTileBlockerFromPosition(x, y, out blocker);

                WFCBlocker wfcBlocker = blocker as WFCBlocker;
                int stateCount = wfcBlocker.GetPossibleStatesCount();

                List<Vector2Int> neighbors = new List<Vector2Int>
                {
                    
                    new Vector2Int(x, y - 1)  // below
                };

                foreach(var neighbor in neighbors)
                {
                    if (IsInGrid(neighbor.x, neighbor.y))
                    {
                        ITileblocker neighborBlocker;
                        grid.GetTileBlockerFromPosition(neighbor.x, neighbor.y, out neighborBlocker);
                        int state = 0;

                        if ((neighborBlocker as Asteroid) != null)
                            state = 1;
                        else if ((neighborBlocker as AsteroidCore) != null)
                            state = 2;
                   

                        WFCBlocker neighborToWfc = neighborBlocker as WFCBlocker;
                        wfcBlocker.UpdatePossibleStatesBasedOnNeighbor(state, neighbor.x, neighbor.y);
                    }
                }
               
            

                if (stateCount > wfcBlocker.GetPossibleStatesCount())
                    change = true;

            }

            
        }

        Debug.Log("COLLAPSED READY");

        collapseRow(y);

        // get element with smallest Entropy
       
    }


    private void Start()
    { 

        SpawnTileBlocker(SpaceGridTileBlocker.PlayerShip, 5, 0);
        

    }

    public void NextTurn()
    {
        TurnMovement();
        MoveProjectiles();
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
                    Debug.Log("Testrn");
                    Debug.Log(toMove.GetGameObject() == null);
                    if(toMove.GetGameObject().GetComponent<PlayerShip>() != null || toMove.GetGameObject().GetComponent<Projectile>() != null)
                    {
                        //Do nothing because we do not move the player or projectiles in turns
                        continue;
                    }
                }         
                MoveTileBlocker(x, y, x, y-1);

            }
        }

        generateNewLine();
    }

    private void SpawnTileBlocker(SpaceGridTileBlocker toSpawn, int _x, int _y)
    {
        ITileblocker tileblocker;

        Vector2 pos = new Vector2(_x, _y);
        switch (toSpawn)
        {
            case SpaceGridTileBlocker.Asteroid:
                tileblocker = SpawnAsteroid();
                tileblocker.SetGridPosition(pos);
                break;
            case SpaceGridTileBlocker.WFCTileBlocker:
                WFCBlocker blocker = Instantiate(wfcBlocker);
                blocker.init(wfcElementCount, wfcConstraints, probability);
                tileblocker = blocker;
                tileblocker.SetGridPosition(pos);
                break;
            case SpaceGridTileBlocker.AsteroidCenter:
                tileblocker = SpawnAsteroidMiddle();
                tileblocker.SetGridPosition(pos);
                break;
            case SpaceGridTileBlocker.PlayerShip:
                tileblocker = SpawnPlayerShip();
                playerShip = tileblocker as PlayerShip;
                break;
            default:
                tileblocker = null;
                break;
                
        }
        ITileblocker old;
        if(grid.SetTileblockerAtPosition(_x, _y, tileblocker, out old))
        {
            //Something was there before - handle it!
        }
        tileblocker.SetGridPosition(new Vector2(_x, _y));
        tileblocker.GetGameObject().transform.position = grid.GetWorldPosition(_x, _y) + new Vector3(grid.cellSize / 2, grid.cellSize / 2, 0);
    }

    private void SpawnTileBlocker(SpaceGridTileBlocker toSpawn, int _x, int _y, Projectile.FlightDirection direction)
    {
        ITileblocker tileblocker;        
        tileblocker = SpawnProjectile(direction);
        ITileblocker old;
        if (grid.SetTileblockerAtPosition(_x, _y, tileblocker, out old))
        {
            //Something was there before - handle it!
            OnCollision(tileblocker, old);

        }
        tileblocker.SetGridPosition(new Vector2(_x, _y));
        tileblocker.GetGameObject().transform.position = grid.GetWorldPosition(_x, _y) + new Vector3(grid.cellSize / 2, grid.cellSize / 2, 0);
    }

    private bool MoveTileBlocker(int oldX, int oldY,int newX, int newY)
    {
        bool result = false;
        ITileblocker tileblocker;
        ITileblocker atPositionBefore = null;

        // Check if we actually have something to move at the position
        if (grid.FreePosition(oldX, oldY, out tileblocker))
        {
            //check if target position is in grid
            if (!IsInGrid(newX, newY))
            {
                DestroyTileBlocker(tileblocker);
                return result;
            }

            if(grid.GetTileBlockerFromPosition(newX, newY, out atPositionBefore))
            {
                OnCollision(tileblocker, atPositionBefore);
                if(tileblocker as PlayerShip!= null)
                {
                    grid.SetTileblockerAtPosition(newX, newY, tileblocker, out atPositionBefore);
                    tileblocker.SetGridPosition(new Vector2(newX, newY));
                    tileblocker.GetGameObject().transform.position = grid.GetWorldPosition(newX, newY) + new Vector3(grid.cellSize / 2, grid.cellSize / 2, 0);
                }
                return result;

            }
            
            grid.SetTileblockerAtPosition(newX, newY, tileblocker, out atPositionBefore);
            tileblocker.SetGridPosition(new Vector2(newX, newY));
            tileblocker.GetGameObject().transform.position = grid.GetWorldPosition(newX, newY) + new Vector3(grid.cellSize / 2, grid.cellSize / 2, 0);
            //Debug.Log("Moved tileblocker to " + newX + "," + newY);
        }
        return result;
    }

    private ITileblocker SpawnAsteroid()
    {
        Asteroid newAsteroid = Instantiate(asteroidPrefab);
        return newAsteroid;
    }

    private ITileblocker SpawnAsteroidMiddle()
    {
        AsteroidCore newAst = Instantiate(asteroid2);
        return newAst;
    }

    private ITileblocker SpawnPlayerShip()
    {
        PlayerShip newPlayerShip = Instantiate(playerShipPrefab);
        return newPlayerShip;
    }

    private ITileblocker SpawnProjectile(Projectile.FlightDirection direction)
    {
        Projectile newProjectile = Instantiate(projectilePrefab);
        newProjectile.SetFlightDirection(direction);
        projectiles.Add(newProjectile);
        return newProjectile;
    }

    private bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < grid.width && y < grid.height;
    }

    private void OnCollision(ITileblocker moving, ITileblocker stationary)
    {
        //if either the thing we move or the thing we collide with is the player ship
        if (stationary.GetGameObject().GetComponent<PlayerShip>() != null)
        {
            //interact!!!
            OnShipCollidedWithTileBlocker(moving);
            return;
        }
        if (moving.GetGameObject().GetComponent<PlayerShip>() != null)
        {
            OnShipCollidedWithTileBlocker(stationary);            
            return;
        }
        DestroyTileBlocker(moving);
        DestroyTileBlocker(stationary);        
    }

    private void OnShipCollidedWithTileBlocker(ITileblocker other)
    {
        Debug.Log("Ship hit Asteroid time before getComponent: " + Time.timeAsDouble);
        if (other.GetGameObject().GetComponent<Asteroid>() != null)
        {
            Debug.Log("Ship hit Asteroid time: " + Time.timeAsDouble);
            DestroyTileBlocker(other);
            GameManager.Instance.PlayerHitByAsteroid();
        }        
    }

    

    private void DestroyTileBlocker(ITileblocker tileblocker)
    {
        int x = (int)tileblocker.GetGridPosition().x;
        int y = (int)tileblocker.GetGridPosition().y;
        if (IsInGrid(x, y))
        {
            //Debug.Log("Freeing grid position " + x + "," + y);
            ITileblocker old;
            grid.FreePosition(x,y, out old);
        }        
        Destroy(tileblocker.GetGameObject());
    }

    private void MoveProjectiles()
    {
        projectiles.RemoveAll(s => s == null);
        foreach (var projectile in projectiles) 
        {
            switch (projectile.direction)
            {
                case Projectile.FlightDirection.UP:
                    {                        
                        MoveTileBlocker(projectile.xPosition, projectile.yPosition, projectile.xPosition, projectile.yPosition + 1);
                        break;
                    }
                case Projectile.FlightDirection.DOWN:
                    {
                        MoveTileBlocker(projectile.xPosition, projectile.yPosition, projectile.xPosition, projectile.yPosition - 1);
                        if(projectile != null) 
                        {
                            MoveTileBlocker(projectile.xPosition, projectile.yPosition, projectile.xPosition, projectile.yPosition - 1);
                        }
                        break;
                    }
            }
        }
    }

    public void MovePlayer(Card.Type card)
    {
        switch (card)
        {
            case Card.Type.LEFT:
                {
                    //Debug.Log("Moving player left from position: " + playerShip.xPosition + " " + playerShip.yPosition);
                    int newX = playerShip.xPosition - 1;
                    if (newX < 0)
                    {
                        newX = grid.width - 1;
                    }
                    MoveTileBlocker(playerShip.xPosition, playerShip.yPosition, newX, playerShip.yPosition);
                    break;
                }
                
            case Card.Type.RIGHT:
                {
                    //Debug.Log("Moving player right from position: " + playerShip.xPosition + " " + playerShip.yPosition);
                    int newX = playerShip.xPosition + 1;
                    if (newX >= grid.width)
                    {
                        newX = 0;
                    }
                    MoveTileBlocker(playerShip.xPosition, playerShip.yPosition, newX, playerShip.yPosition);
                    break;
                }
                
            case Card.Type.SHOOT:
                break;
        }
    }    

    public void PlayerShoot()
    {
        SpawnTileBlocker(SpaceGridTileBlocker.Projectile, playerShip.xPosition, playerShip.yPosition + 1, Projectile.FlightDirection.UP);
    }


    public enum SpaceGridTileBlocker
    {
        Asteroid,
        AsteroidCenter,
        
        PlayerShip,
        Projectile,

        WFCTileBlocker,
    }


}
