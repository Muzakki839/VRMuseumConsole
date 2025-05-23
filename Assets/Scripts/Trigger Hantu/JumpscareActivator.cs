using UnityEngine;

public class JumpscareActivator : MonoBehaviour
{
    [Header("Activation Settings")]
    public GameObject targetObjectToActivate;
    public bool playMovement = true;

    [Header("Audio Settings")]
    public AudioClip jumpscareSound;
    public AudioSource jumpscareAudioSource; // Attach di scene/objek (opsional)

    [Header("Material Swap Settings")]
    public Renderer targetRenderer;
    public Material newMaterial;
    public int materialIndexToSwap = 0;

    private Material[] originalMaterials;

    private void Start()
    {
        if (targetRenderer != null)
        {
            originalMaterials = targetRenderer.materials;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.name);
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player entered the trigger.");

        ActivateTargetObject();
        SwapMaterial();
        PlayJumpscareSound();

        Destroy(gameObject); // Trigger hanya sekali
    }

    private void ActivateTargetObject()
    {
        if (targetObjectToActivate == null) return;

        targetObjectToActivate.SetActive(true);
        Debug.Log("Activated object: " + targetObjectToActivate.name);

        if (playMovement)
        {
            var mover = targetObjectToActivate.GetComponent<JumpscareMover>();
            if (mover != null)
            {
                Debug.Log("Found JumpscareMover on " + targetObjectToActivate.name);
                mover.StartMoving();
            }
            else
            {
                Debug.LogWarning("No JumpscareMover found on " + targetObjectToActivate.name);
            }
        }
    }

    private void SwapMaterial()
    {
        if (targetRenderer == null || newMaterial == null) return;

        var mats = targetRenderer.materials;
        if (materialIndexToSwap < 0 || materialIndexToSwap >= mats.Length)
        {
            Debug.LogWarning("Invalid material index: " + materialIndexToSwap);
            return;
        }

        mats[materialIndexToSwap] = newMaterial;
        targetRenderer.materials = mats;
        Debug.Log("Material swapped to: " + newMaterial.name + " at index " + materialIndexToSwap);
    }

    private void PlayJumpscareSound()
    {
        if (jumpscareSound == null) return;

        if (jumpscareAudioSource != null)
        {
            jumpscareAudioSource.clip = jumpscareSound;
            jumpscareAudioSource.Play();
            Debug.Log("Played jumpscare sound using AudioSource.");
        }
        else
        {
            AudioSource.PlayClipAtPoint(jumpscareSound, transform.position);
            Debug.Log("Played jumpscare sound using PlayClipAtPoint.");
        }
    }
}
