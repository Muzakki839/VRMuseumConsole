using System.Collections;
using UnityEngine;

public class BGMLoop : MonoBehaviour
{
    public AudioClip[] bgmList;
    public AudioSource audioSource;
    public float fadeDuration = 1.5f; // Durasi fade in/out dalam detik

    private int currentIndex = 0;
    private bool isTransitioning = false;

    void Start()
    {
        if (bgmList.Length > 0 && audioSource != null)
        {
            PlayCurrentBGM();
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying && !isTransitioning)
        {
            StartCoroutine(FadeToNextBGM());
        }
    }

    void PlayCurrentBGM()
    {
        audioSource.clip = bgmList[currentIndex];
        audioSource.Play();
    }

    IEnumerator FadeToNextBGM()
    {
        isTransitioning = true;

        // Fade out
        float startVolume = audioSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        // Ganti lagu
        currentIndex = (currentIndex + 1) % bgmList.Length;
        audioSource.clip = bgmList[currentIndex];
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = startVolume;
        isTransitioning = false;
    }
}
