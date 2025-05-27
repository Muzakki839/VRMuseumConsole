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

    void Start()
    {
        if (vignetteController == null)
        {
            Debug.LogError("TunnelingVignetteController belum di-assign!");
            return;
        }

        // Inisialisasi slider dengan nilai awal dari defaultParameters
        apertureSlider.value = vignetteController.defaultParameters.apertureSize;
        featherSlider.value = vignetteController.defaultParameters.featheringEffect;

        // Tambahkan listener untuk slider
        apertureSlider.onValueChanged.AddListener(UpdateAperture);
        featherSlider.onValueChanged.AddListener(UpdateFeather);

        // Toggle listener
        if (vignetteToggle != null)
        {
            vignetteToggle.isOn = TunellingVignette.gameObject.activeSelf;
            vignetteToggle.onValueChanged.AddListener(ToggleVignette);
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
}
