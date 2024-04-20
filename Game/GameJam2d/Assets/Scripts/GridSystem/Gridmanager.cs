using Debaka.Utils;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;

public class Gridmanager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroid;
    [SerializeField] private WFCBlocker wfcBlocker;
    public static Gridmanager Instance { get; private set; }

    // array that represents tile map:
    private int[,] wfcGridArray;
    private int wfcElementCount;

    private Grid grid;

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


        wfcGridArray = new int[,]
        {
            { 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0 }
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

        Debug.Log("spawned");

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
                Debug.Log(grid.FreePosition(x, y, out blocker));
                Destroy(blocker.GetGameObject());
            }
        }
       
        // now iterate through remaining rows and do wfc collapse
        for(int y = empytRows; y < grid.height; y++)
        {
            collapseRow(y);
        }
        
    }

    private void collapseRow(int row)
    {
        Debug.Log("Collapseing ------------------------------------");
        Debug.Log("Row " + row);
        Debug.Log(collapsed(row));

       
         while (!collapsed(row))
         {
             WFCBlocker lowestEntropy = getLowestEntropy(row);

             lowestEntropy.collapse();

            // updateNeighborsConstraints(lowestEntropy, row);
         }

          // update visuals:

          for(int x = 0; x < grid.width; x++)
          {
              ITileblocker blocker = null;
              Debug.Log(grid.FreePosition(x, row, out blocker));

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
                  default:
                      break;
              }
          }
          
    }

    private void updateNeighborsConstraints(WFCBlocker collapsedBlocker, int row)
    {
        int x = collapsedBlocker.xPosition;
        int y = collapsedBlocker.yPosition;

        // Neighboring positions to check: left, right, above, below
        List<Vector2Int> neighbors = new List<Vector2Int>
    {
        new Vector2Int(x - 1, y), // left
        new Vector2Int(x + 1, y), // right
        new Vector2Int(x, y + 1)  // below
    };

        foreach (var neighbor in neighbors)
        {
            if (IsInGrid(neighbor.x, neighbor.y))
            {
                grid.GetTileBlockerFromPosition(neighbor.x, neighbor.y, out ITileblocker neighborBlocker);
                WFCBlocker wfcNeighbor = neighborBlocker as WFCBlocker;
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

            Debug.Log(wfcBlocker.GetState() + "Was the State)");

            collapsed = collapsed && (wfcBlocker.GetState() != -1);
        }
        Debug.Log(collapsed);
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


    private void Start()
    {
        //SpawnTileBlocker(SpaceGridTileBlocker.Asteroid, 0, grid.height - 1);   
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
            case SpaceGridTileBlocker.WFCTileBlocker:
                WFCBlocker blocker = Instantiate(wfcBlocker);
                blocker.init(wfcElementCount, wfcConstraints);
                tileblocker = blocker;
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
        WFCTileBlocker

    }


}
