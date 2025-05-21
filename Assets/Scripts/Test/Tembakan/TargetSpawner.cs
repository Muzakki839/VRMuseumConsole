using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TargetEntry
{
    public GameObject prefab;
    [Range(0f, 1f)] public float spawnChance;
}

public class TargetSpawner : MonoBehaviour
{
    [Header("Target Configuration")]
    public TargetEntry[] targetEntries;

    [Header("Spawn Points")]
    public Transform[] popupPoints;
    public Transform[] moverStartPoints;

    [Header("Spawn Settings")]
    public float popupSpeed = 3f;
    public float moverSpeed = 2f;
    public float interval = 1.5f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            GameObject prefab = GetRandomTarget();
            if (prefab == null) continue;

            bool useMover = Random.value < 0.2f; // 20% chance mover
            if (useMover)
                SpawnMover(prefab);
            else
                SpawnPopup(prefab);
        }
    }

    GameObject GetRandomTarget()
    {
        float roll = Random.value;
        float cumulative = 0f;

        foreach (var entry in targetEntries)
        {
            cumulative += entry.spawnChance;
            if (roll <= cumulative)
                return entry.prefab;
        }

        return null;
    }

    void SpawnPopup(GameObject prefab)
    {
        List<Transform> freeSlots = new List<Transform>();

        foreach (var point in popupPoints)
        {
            PopupSlot slot = point.GetComponent<PopupSlot>();
            if (slot != null && !slot.IsOccupied)
            {
                freeSlots.Add(point);
            }
        }

        if (freeSlots.Count == 0) return;

        Transform spawnPoint = freeSlots[Random.Range(0, freeSlots.Count)];
        GameObject target = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        TargetPopup popup = target.AddComponent<TargetPopup>();
        popup.Init();
        spawnPoint.GetComponent<PopupSlot>().SetOccupied(true);
        popup.onHide = () => spawnPoint.GetComponent<PopupSlot>().SetOccupied(false);
    }

    void SpawnMover(GameObject prefab)
    {
        bool fromLeft = Random.value > 0.5f;
        Transform spawnPoint = fromLeft ? moverStartPoints[0] : moverStartPoints[1];
        Vector3 dir = fromLeft ? Vector3.right : Vector3.left;

        GameObject target = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        target.AddComponent<TargetMover>().Init(dir, moverSpeed);
    }
}
