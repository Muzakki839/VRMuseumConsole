using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private const string MASTER_VOLUME_PARAM = "MasterVolume";
    private const string MUSIC_VOLUME_PARAM = "MusicVolume";
    private const string SFX_VOLUME_PARAM = "SFXVolume";

    private void Start()
    {
        // Load saved values, atau set default ke slider (1 = 0dB)
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_VOLUME_PARAM, 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_PARAM, 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_PARAM, 0.75f);

        // Pasang listener slider
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Terapkan setting ke mixer
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    public void SetMasterVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(MASTER_VOLUME_PARAM, dB);
        PlayerPrefs.SetFloat(MASTER_VOLUME_PARAM, sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(MUSIC_VOLUME_PARAM, dB);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PARAM, sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(SFX_VOLUME_PARAM, dB);
        PlayerPrefs.SetFloat(SFX_VOLUME_PARAM, sliderValue);
    }
}
