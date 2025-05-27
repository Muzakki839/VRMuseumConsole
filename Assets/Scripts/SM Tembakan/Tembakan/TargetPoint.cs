using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public bool isBeingDestroyed { get; private set; } = false;
    public int pointValue = 1;

    [Header("Audio")]
    public AudioClip hitSound;          // Audio clip untuk suara kena tembak
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnShot()
    {
        if (isBeingDestroyed) return;
        isBeingDestroyed = true;

        // Play hit sound sebelum dihancurkan
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        SM_Tembakan_Manager.Instance.AddScore(pointValue);

        // Jika mau delay sedikit agar suara terdengar sebelum Destroy, bisa pakai coroutine
        Destroy(gameObject, hitSound != null ? hitSound.length : 0f);
    }
}
