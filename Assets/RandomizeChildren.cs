using UnityEngine;

public class RandomizeChildren : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float minYRotation = 0f;
    public float maxYRotation = 360f;

    [Header("Scale Settings")]
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    [ContextMenu("Randomize Children")]
    void Randomize()
    {
        foreach (Transform child in transform)
        {
            // Random Y-axis rotation (horizontal rotation, bagus buat pohon)
            float randomY = Random.Range(minYRotation, maxYRotation);
            child.localRotation = Quaternion.Euler(0f, randomY, 0f);

            // Random uniform scale
            float randomScale = Random.Range(minScale, maxScale);
            child.localScale = new Vector3(randomScale, randomScale, randomScale);
        }
    }
}
