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
    public PopupTargetSlot[] popupSlots;

    [Header("Spawn Settings")]
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
        List<PopupTargetSlot> freeSlots = new List<PopupTargetSlot>();

        foreach (var slot in popupSlots)
        {
            if (!slot.IsOccupied)
            {
                freeSlots.Add(slot);
            }
        }

        if (freeSlots.Count == 0) return;

        PopupTargetSlot chosenSlot = freeSlots[Random.Range(0, freeSlots.Count)];
        chosenSlot.Spawn(prefab);
    }
}
