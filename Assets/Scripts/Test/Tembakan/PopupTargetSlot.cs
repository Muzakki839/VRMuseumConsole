using System.Collections;
using UnityEngine;

public class PopupTargetSlot : MonoBehaviour
{
    [Header("Popup Config")]
    public float popupHeight = 1.5f;
    public float popupSpeed = 3f;
    public float visibleDuration = 2f;

    private bool isOccupied = false;
    private GameObject currentTarget;
    private Coroutine popupCoroutine;

    public bool IsOccupied => isOccupied;

    public void Spawn(GameObject prefab)
    {
        if (isOccupied) return;

        isOccupied = true;
        Vector3 spawnPos = transform.position;

        currentTarget = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
        popupCoroutine = StartCoroutine(PopupRoutine(currentTarget));
    }

    IEnumerator PopupRoutine(GameObject target)
    {
        if (target == null) yield break;

        Vector3 startPos = target.transform.position;
        Vector3 targetPos = startPos + Vector3.up * popupHeight;

        // Naik
        float t = 0f;
        while (t < 1f)
        {
            if (target == null) yield break;

            t += Time.deltaTime * popupSpeed;
            target.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(visibleDuration);

        // Turun
        t = 0f;
        while (t < 1f)
        {
            if (target == null) yield break;

            t += Time.deltaTime * popupSpeed;
            target.transform.position = Vector3.Lerp(targetPos, startPos, t);
            yield return null;
        }

        if (target != null) Destroy(target);
        currentTarget = null;
        isOccupied = false;
        popupCoroutine = null;
    }

    public void ForceHide()
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
            popupCoroutine = null;
        }

        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }

        isOccupied = false;
    }
}
