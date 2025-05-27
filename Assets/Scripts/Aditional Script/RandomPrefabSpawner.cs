using UnityEngine;

public class RandomColorSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] prefabsWithColor;     // Prefab yang akan diwarnai
    public GameObject[] prefabsWithoutColor;  // Prefab yang tidak diwarnai
    public Transform spawnPoint;              // Titik pusat spawn
    public int spawnCount = 5;                // Total objek yang di-spawn
    public float spawnRadius = 2f;            // Radius area spawn acak

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnRandomCharacter();
        }
    }

    void SpawnRandomCharacter()
    {
        // Tentukan apakah spawn dari array withColor atau withoutColor
        bool useColor = Random.value < 0.5f; // 50% chance (bisa diubah jadi parameter juga)
        GameObject[] sourceArray = useColor ? prefabsWithColor : prefabsWithoutColor;

        if (sourceArray.Length == 0)
        {
            Debug.LogWarning("No prefabs available in selected group.");
            return;
        }

        // Pilih prefab secara acak dari array yang sesuai
        GameObject selectedPrefab = sourceArray[Random.Range(0, sourceArray.Length)];

        // Posisi acak sekitar spawnPoint
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0,
            Random.Range(-spawnRadius, spawnRadius)
        );
        Vector3 spawnPos = spawnPoint.position + randomOffset;

        // Spawn
        GameObject obj = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

        // Jika prefab dari group yang perlu dirandom warnanya
        if (useColor)
        {
            Renderer rend = obj.GetComponentInChildren<SkinnedMeshRenderer>();
            if (rend == null)
                rend = obj.GetComponentInChildren<Renderer>();

            if (rend != null)
            {
                ApplyRandomColorToClothes(rend);
            }
            else
            {
                Debug.LogWarning($"{obj.name} doesn't have a Renderer.");
            }
        }
    }

    void ApplyRandomColorToClothes(Renderer rend)
    {
        Material[] mats = rend.materials;

        for (int i = 0; i < mats.Length; i++)
        {
            string cleanedName = mats[i].name.ToLower().Replace(" (instance)", "").Trim();

            if (cleanedName == "kulit_manusia")
                continue;

            // Warna terang
            mats[i].color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.6f, 1f);
        }

        rend.materials = mats;
    }
}
