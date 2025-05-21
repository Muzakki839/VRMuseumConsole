using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzleTransform;
    public float bulletForce = 500f;

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzleTransform.position, Quaternion.identity);

        // Koreksi rotasi prefab bullet dari Y+ ke Z+
        Quaternion correction = Quaternion.Euler(90, 0, 0); // Rotasi agar Z+ jadi depan
        bullet.transform.rotation = muzzleTransform.rotation * correction;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(muzzleTransform.forward * bulletForce, ForceMode.Impulse);

        Destroy(bullet, 5f);
    }
}
