using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public bool isBeingDestroyed { get; private set; } = false;
    public int pointValue = 1;

    public void OnShot()
    {
        if (isBeingDestroyed) return;
        isBeingDestroyed = true;
        GameManager.Instance.AddScore(pointValue);
        Destroy(gameObject);
    }
}
