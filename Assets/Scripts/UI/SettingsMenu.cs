using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject controlsPanel; //Controls inside the settings menu
    [SerializeField] private Button closeSettingsButton, openControlsButton, closeControlsButton;
    // Start is called before the first frame update
    void Start()
    {
        closeSettingsButton.onClick.AddListener(CloseSettings);
        openControlsButton.onClick.AddListener(OpenControls);
        closeControlsButton.onClick.AddListener(CloseControls);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CloseSettings()
    {
        settingsMenu.SetActive(false);
    }

      private void OpenControls()
    {
        controlsPanel.SetActive(true);
    }

      private void CloseControls()
    {
        controlsPanel.SetActive(false);
    }
}
