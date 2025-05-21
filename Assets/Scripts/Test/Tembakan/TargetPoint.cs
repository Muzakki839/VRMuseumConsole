using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public int pointValue;

    public void OnShot()
    {
        GameManager.Instance.AddScore(pointValue);
        Destroy(gameObject);
    }
}
