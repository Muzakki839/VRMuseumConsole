using UnityEngine;

public class JumpscareActivator : MonoBehaviour
{
    [Header("Activation Settings")]
    public GameObject targetObjectToActivate;
    public bool playMovement = false;

    [Header("Destruction Settings")]
    public bool isDestroyAfterDelay = false;
    public float destroyDelay = 1f;

    [Header("Audio Settings")]
    public AudioClip jumpscareSound;
    public AudioSource jumpscareAudioSource; // Attach di scene/objek (opsional)

    [Header("Material Swap Settings")]
    public Renderer targetRenderer;
    public Material newMaterial;
    public int materialIndexToSwap = 0;


    private Material[] originalMaterials;
    private void Awake()
    {
        targetObjectToActivate.SetActive(false);
    }
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

        if (isDestroyAfterDelay)
        {
            Destroy(gameObject, destroyDelay);
            Debug.Log("Object will be destroyed after delay: " + destroyDelay + " seconds.");
        }
        else
        {
            Destroy(gameObject); // Trigger hanya sekali
            Debug.Log("Object destroyed immediately.");
        }
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
