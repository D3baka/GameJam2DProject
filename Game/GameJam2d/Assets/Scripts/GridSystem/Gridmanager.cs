using UnityEngine;

public class Gridmanager : MonoBehaviour
{
    [SerializeField] private Asteroid toSpawn;
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
            Debug.LogError("Too many GameManager Instances: " + Gridmanager.Instance);
        }
        Vector3 offset = Camera.main.ScreenToWorldPoint(new Vector3(0,0,10));
        grid = new Grid(11, 11, 15, offset);
    }

    private void Start()
    {   
        Asteroid newAsteroid = Instantiate(toSpawn);
        ITileblocker old;
        grid.SetTileblockerAtPosition( 0,0, newAsteroid, out old );
        newAsteroid.transform.position = grid.GetWorldPosition(0,0) + new Vector3(7.5f,7.5f,0);
       
    }
}
