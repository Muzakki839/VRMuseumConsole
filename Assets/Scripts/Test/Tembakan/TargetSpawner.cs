using System.Collections;
using UnityEngine;

[System.Serializable]
public class TargetEntry
{
    public GameObject prefab;
    [Range(0f, 1f)] public float spawnChance = 1f; // Probabilitas muncul (0–1)
}


public class TargetSpawner : MonoBehaviour
{
    [Header("Target Prefabs")]
    public TargetEntry[] targetEntries;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;

    [Header("Animation Settings")]
    public float popupHeight = 1.5f;
    public float popupSpeed = 3f;

    private GameObject currentTarget;
    private bool spawnRequestedEarly = false;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float timer = 0f;

            // Tunggu spawn interval atau hingga ada request spawn lebih awal
            while (timer < spawnInterval && !spawnRequestedEarly)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            spawnRequestedEarly = false;

            // Jika target lama masih ada dan belum dihancurkan
            if (currentTarget != null)
            {
                TargetPoint tp = currentTarget.GetComponent<TargetPoint>();
                if (tp != null && !tp.isBeingDestroyed)
                {
                    yield return StartCoroutine(HideTarget(currentTarget));
                    Destroy(currentTarget);
                }
            }

            // Spawn baru
            GameObject prefab = GetRandomTargetPrefab();
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            currentTarget = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            yield return StartCoroutine(ShowTarget(currentTarget));
        }
    }

    private GameObject GetRandomTargetPrefab()
    {
        float total = 0f;
        foreach (var entry in targetEntries)
            total += entry.spawnChance;

        float random = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var entry in targetEntries)
        {
            cumulative += entry.spawnChance;
            if (random <= cumulative)
                return entry.prefab;
        }

        return targetEntries[0].prefab; // fallback
    }

    public void ForceNextSpawn()
    {
        spawnRequestedEarly = true;
    }

    private IEnumerator ShowTarget(GameObject target)
    {
        if (target == null) yield break;

        Vector3 end = target.transform.position;
        Vector3 start = end + Vector3.down * popupHeight;

        float t = 0f;
        target.transform.position = start;

        while (t < 1f)
        {
            if (target == null) yield break;

            t += Time.deltaTime * popupSpeed;
            target.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }

    private IEnumerator HideTarget(GameObject target)
    {
        if (target == null) yield break;

        Vector3 start = target.transform.position;
        Vector3 end = start + Vector3.down * popupHeight;

        float t = 0f;

        while (t < 1f)
        {
            if (target == null) yield break;

            t += Time.deltaTime * popupSpeed;
            target.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }

}
