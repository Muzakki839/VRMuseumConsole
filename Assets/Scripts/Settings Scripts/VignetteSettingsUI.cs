using UnityEngine;
using UnityEngine.UI;

public class VignetteSettingsUI : MonoBehaviour
{
    [Header("References")]
    public Transform TunellingVignette;

    [Header("UI Sliders")]
    public Slider apertureSlider;
    public Slider featherSlider;

    [Header("UI Toggle")]
    public Toggle vignetteToggle;

    [Header("Tunneling Vignette")]
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort.TunnelingVignetteController vignetteController;

    private const string ApertureKey = "VignetteAperture";
    private const string FeatherKey = "VignetteFeather";
    private const string ToggleKey = "VignetteToggle";

    void Start()
    {
        if (vignetteController == null)
        {
            Debug.LogError("TunnelingVignetteController belum di-assign!");
            return;
        }

        // Load dari PlayerPrefs
        float savedAperture = PlayerPrefs.GetFloat(ApertureKey, 0.5f);
        float savedFeather = PlayerPrefs.GetFloat(FeatherKey, 0.5f);
        bool savedToggle = PlayerPrefs.GetInt(ToggleKey, 1) == 1;

        apertureSlider.value = savedAperture;
        featherSlider.value = savedFeather;
        if (vignetteToggle != null)
        {
            vignetteToggle.isOn = savedToggle;
            TunellingVignette.gameObject.SetActive(savedToggle);
        }

        UpdateAperture(savedAperture);
        UpdateFeather(savedFeather);

        apertureSlider.onValueChanged.AddListener((v) =>
        {
            UpdateAperture(v);
            SaveSettings();
            if (!TunellingVignette.gameObject.activeSelf)
                TunellingVignette.gameObject.SetActive(true);
            if (vignetteToggle != null) vignetteToggle.isOn = true;
        });

        featherSlider.onValueChanged.AddListener((v) =>
        {
            UpdateFeather(v);
            SaveSettings();
            if (!TunellingVignette.gameObject.activeSelf)
                TunellingVignette.gameObject.SetActive(true);
            if (vignetteToggle != null) vignetteToggle.isOn = true;
        });

        if (vignetteToggle != null)
        {
            vignetteToggle.onValueChanged.AddListener((isOn) =>
            {
                ToggleVignette(isOn);
                SaveSettings();
            });
        }
    }

    void UpdateAperture(float value)
    {
        var parameters = vignetteController.defaultParameters;
        parameters.apertureSize = 1f - Mathf.Clamp01(value);  // dibalik
        vignetteController.defaultParameters = parameters;
    }

    void UpdateFeather(float value)
    {
        var parameters = vignetteController.defaultParameters;
        parameters.featheringEffect = Mathf.Clamp01(value);
        vignetteController.defaultParameters = parameters;
    }

    void ToggleVignette(bool isOn)
    {
        if (TunellingVignette != null)
        {
            TunellingVignette.gameObject.SetActive(isOn);
        }
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat(ApertureKey, apertureSlider.value);
        PlayerPrefs.SetFloat(FeatherKey, featherSlider.value);
        PlayerPrefs.SetInt(ToggleKey, TunellingVignette.gameObject.activeSelf ? 1 : 0);
        PlayerPrefs.Save();
    }
}
