using UnityEngine;

public class Gridmanager : MonoBehaviour
{
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
        grid = new Grid(20, 20, 20, offset);
    }   
}
