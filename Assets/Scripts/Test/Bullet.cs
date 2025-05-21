using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public TrailRenderer trailPrefab;
    private TrailRenderer trailInstance;

    private void Start()
    {
        // Tambahkan TrailRenderer jika prefab tersedia dan belum ada
        if (trailPrefab != null && trailInstance == null)
        {
            trailInstance = Instantiate(trailPrefab, transform.position, Quaternion.identity, transform);
            trailInstance.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TargetPoint target = other.GetComponent<TargetPoint>();
        if (target != null)
        {
            target.OnShot();

            // Jika ada slot, suruh dia menghancurkan target dengan benar
            PopupTargetSlot slot = target.GetComponentInParent<PopupTargetSlot>();
            if (slot != null)
            {
                slot.ForceHide();
            }
        }

        Destroy(gameObject); // Hancurkan peluru apapun yang ditabrak
    }
}
