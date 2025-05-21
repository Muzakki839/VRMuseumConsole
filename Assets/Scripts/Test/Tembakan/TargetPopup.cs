using System.Collections;
using UnityEngine;

public class TargetPopup : MonoBehaviour
{
    public float popupHeight = 1.5f;
    public float popupSpeed = 3f;
    public float visibleDuration = 2f;

    private Vector3 originalPos;
    public System.Action onHide;

    public void Init()
    {
        originalPos = transform.position;
        StartCoroutine(PopupRoutine());
    }

    IEnumerator PopupRoutine()
    {
        Vector3 targetPos = originalPos + Vector3.up * popupHeight;

        // Naik
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * popupSpeed;
            transform.position = Vector3.Lerp(originalPos, targetPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(visibleDuration);

        // Turun
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * popupSpeed;
            transform.position = Vector3.Lerp(targetPos, originalPos, t);
            yield return null;
        }

        onHide?.Invoke();
        Destroy(gameObject);
    }

    public void OnShot()
    {
        onHide?.Invoke();
        Destroy(gameObject);
    }
}
