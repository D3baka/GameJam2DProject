using UnityEngine;

public interface ITileblocker
{
    public Vector2 GetGridPosition();
    public GameObject GetGameObject();

    public void SetGridPosition(Vector2 position);
}
