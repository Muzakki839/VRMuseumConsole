using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah objek yang terkena punya komponen TargetPoint
        TargetPoint target = other.GetComponent<TargetPoint>();
        if (target != null)
        {
            target.OnShot(); // Tambah skor & hancurkan target
            Destroy(gameObject); // Hancurkan peluru
        }
    }
}
