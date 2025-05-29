using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject prefabToSpawn;
    public int count = 5;
    public float radius = 5f;
    public Transform spawnPoint;
    public Transform CenterPoint;

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        if (prefabToSpawn == null || spawnPoint == null || CenterPoint == null)
        {
            Debug.LogWarning("Prefab, spawnPoint, atau CenterPoint belum di-assign!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = spawnPoint.position + Random.insideUnitSphere * radius;
            randomPos.y = spawnPoint.position.y; // agar tetap di permukaan

            GameObject npc = Instantiate(prefabToSpawn, randomPos, Quaternion.identity);

            // Cari komponen BasicNavMesh dan set wanderCenter
            BasicNavMesh navScript = npc.GetComponent<BasicNavMesh>();
            if (navScript != null)
            {
                navScript.wanderCenter = CenterPoint;
            }
            else
            {
                Debug.LogWarning($"Prefab {prefabToSpawn.name} tidak punya komponen BasicNavMesh!");
            }
        }
    }
}
