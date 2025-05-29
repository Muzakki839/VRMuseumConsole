using UnityEngine;

public class BGMLoop : MonoBehaviour
{
    public AudioClip[] bgmList; // Daftar lagu
    public AudioSource audioSource; // Komponen AudioSource
    private int currentIndex = 0;

    void Start()
    {
        if (bgmList.Length > 0 && audioSource != null)
        {
            PlayCurrentBGM();
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextBGM();
        }
    }

    void PlayCurrentBGM()
    {
        audioSource.clip = bgmList[currentIndex];
        audioSource.Play();
    }

    void PlayNextBGM()
    {
        currentIndex = (currentIndex + 1) % bgmList.Length; // Loop ke awal saat selesai
        PlayCurrentBGM();
    }
}
