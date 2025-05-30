using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [Header("Gun Setup")]
    public GameObject bulletPrefab;
    public Transform muzzleTransform;
    public float bulletForce = 500f;

    [Header("Effects")]
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem muzzleSmokeEffect;

    public void Shoot()
    {
        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, muzzleTransform.position, muzzleTransform.rotation);

        // Apply force
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = muzzleTransform.forward * bulletForce;
        }

        // Play muzzle flash
        if (muzzleFlashEffect != null)
        {
            muzzleFlashEffect.transform.position = muzzleTransform.position;
            muzzleFlashEffect.transform.rotation = muzzleTransform.rotation;
            muzzleFlashEffect.Play();
        }

        // Play muzzle smoke
        if (muzzleSmokeEffect != null)
        {
            muzzleSmokeEffect.transform.position = muzzleTransform.position;
            muzzleSmokeEffect.transform.rotation = muzzleTransform.rotation;
            muzzleSmokeEffect.Play();
        }

        // Destroy bullet after delay
        Destroy(bullet, 5f);
    }
}
