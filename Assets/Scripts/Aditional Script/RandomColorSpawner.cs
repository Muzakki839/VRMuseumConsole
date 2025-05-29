using UnityEngine;

public class RandomColorSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] prefabsWithColor;     // Prefab yang akan diwarnai
    public Transform spawnPoint;              // Titik pusat spawn
    public Transform centerPoint;              // Titik pusat spawn
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
        if (prefabsWithColor.Length == 0)
        {
            Debug.LogWarning("No prefabs available in prefabsWithColor.");
            return;
        }

        // Pilih prefab secara acak
        GameObject selectedPrefab = prefabsWithColor[Random.Range(0, prefabsWithColor.Length)];

        // Posisi acak sekitar spawnPoint
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0,
            Random.Range(-spawnRadius, spawnRadius)
        );
        Vector3 spawnPos = spawnPoint.position + randomOffset;

        // Spawn
        GameObject obj = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

        // Terapkan warna acak
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

        // Pasang AI wander
        var ai = obj.GetComponent<NavMeshWander>();
        if (ai != null)
        {
            ai.wanderCenter = centerPoint;
        }
        else
        {
            Debug.LogWarning($"{obj.name} doesn't have NavMeshWander component.");
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
