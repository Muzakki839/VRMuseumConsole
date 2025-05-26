using UnityEngine;

public class RandomPrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;             // Isi dengan beberapa prefab
    public Material[] possibleMaterials;     // Isi dengan 3 material yang bisa dirandom
    public Transform spawnPoint;             // Posisi spawn
    public int spawnCount = 1;               // Berapa kali spawn

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnRandomPrefab();
        }
    }

    void SpawnRandomPrefab()
    {
        if (prefabs.Length == 0 || possibleMaterials.Length == 0) return;

        // Pilih prefab acak
        GameObject selectedPrefab = prefabs[Random.Range(0, prefabs.Length)];

        // Spawn prefab
        GameObject obj = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

        // Ambil renderer
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend == null)
        {
            rend = obj.GetComponentInChildren<Renderer>(); // fallback ke anaknya
        }

        if (rend != null)
        {
            Material[] mats = rend.materials;

            if (mats.Length >= 4)
            {
                int indexToReplace = Random.Range(1, 4); // Index 1–3
                Material randomMat = possibleMaterials[Random.Range(0, possibleMaterials.Length)];

                mats[indexToReplace] = randomMat;
                rend.materials = mats;
            }
            else
            {
                Debug.LogWarning($"{obj.name} doesn't have enough materials (min 4).");
            }
        }
        else
        {
            Debug.LogWarning($"{obj.name} has no Renderer.");
        }
    }
}
