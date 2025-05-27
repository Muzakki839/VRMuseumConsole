using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(AudioSource))]
public class ProximitySoundTrigger : MonoBehaviour
{
    public string playerTag = "Player"; // Tag yang dipakai player
    private AudioSource audioSource;
    private SphereCollider sphereCollider;
    private bool hasPlayed = false;     // supaya sound cuma diputar sekali saat masuk trigger

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !hasPlayed)
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            hasPlayed = false; // reset agar bisa diputar lagi kalau player keluar dan masuk lagi
        }
    }
}
