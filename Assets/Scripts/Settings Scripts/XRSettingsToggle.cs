using UnityEngine;
using UnityEngine.InputSystem;

public class XRSettingsToggle : MonoBehaviour
{
    public InputActionReference toggleSettingsAction;
    public GameObject settingsPanel;

    private void OnEnable()
    {
        toggleSettingsAction.action.Enable();
        toggleSettingsAction.action.performed += OnToggleSettings;
    }

    private void OnDisable()
    {
        toggleSettingsAction.action.performed -= OnToggleSettings;
        toggleSettingsAction.action.Disable();
    }

    private void OnToggleSettings(InputAction.CallbackContext context)
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }
}
