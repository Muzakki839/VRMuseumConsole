using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneNameToLoad;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player triggered the scene change with fade.");
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        // Fade Out (ke hitam)
        yield return StartCoroutine(Fade(0f, 1f));

        // Pindah scene
        SceneManager.LoadScene(sceneNameToLoad);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
