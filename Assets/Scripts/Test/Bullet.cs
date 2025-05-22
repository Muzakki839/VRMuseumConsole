using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Tag Settings")]
    public string targetTag = "Target";

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag(targetTag))
        {
            if (collision.gameObject.TryGetComponent(out TargetPoint target))
            {
                if (!target.isBeingDestroyed)
                {
                    Debug.Log("[Bullet] Valid target hit.");
                    target.OnShot();

                    // Cari spawner dan suruh spawn baru
                    if (FindFirstObjectByType<TargetSpawner>() is TargetSpawner spawner)
                    {
                        spawner.ForceNextSpawn();
                    }
                }
            }
        }

        Destroy(gameObject, 1f);
    }
}
