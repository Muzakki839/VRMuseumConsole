using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [Header("Gun Setup")]
    public GameObject bulletPrefab;
    public Transform muzzleTransform;
    public float bulletForce = 500f;

    public void Shoot()
    {
        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, muzzleTransform.position, muzzleTransform.rotation);

        // Pastikan bullet menghadap ke depan (Z+)
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = muzzleTransform.forward * bulletForce;
        }

        Destroy(bullet, 5f);
    }
}
