using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown fpsDropdown;
    [SerializeField] private Button applyButton;

    Resolution[] resolutions;

    private void Awake()
    {
        GetCurrentSettings();
        
        applyButton.onClick.AddListener(OnApplyButtonClicked);
        vsyncToggle.onValueChanged.AddListener(OnVsyncToggleValueChanged);
    }

    private void GetCurrentSettings()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        vsyncToggle.isOn = QualitySettings.vSyncCount == 1;
        SetUpResolutions();
        SetUpFPS();
        // Disable the FPS dropdown if VSync is on
        fpsDropdown.interactable = !vsyncToggle.isOn;
    }

    private void OnApplyButtonClicked()
    {
        SetVsync(vsyncToggle.isOn);
        SetResolution(resolutionDropdown.options[resolutionDropdown.value].text, fullscreenToggle.isOn);
        SetFPS(fpsDropdown.value);
    }

    private void SetUpResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        HashSet<string> options = new HashSet<string>();

        //add each resolution to the dropdown
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;

            options.Add(option);
        }

        resolutionDropdown.AddOptions(options.ToList());
        //set the current resolution to the current resolution index
        resolutionDropdown.value = options.ToList().IndexOf(Screen.width + "x" + Screen.height);
        resolutionDropdown.RefreshShownValue();
    }

    private void SetUpFPS()
    {
        fpsDropdown.ClearOptions();

        int maxFPS = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);

        // Add FPS options to the SortedSet
        SortedSet<int> options = new SortedSet<int> { 30, 45, 60, 75, 90, 120, 144, 240, maxFPS, maxFPS / 2 };

        // Remove options that exceed the max FPS
        options = new SortedSet<int>(options.Where(option => option <= maxFPS));

        // Add "Unlimited" option
        List<string> finalOptions = options.Select(option => option.ToString()).ToList();
        finalOptions.Add("Uncapped");

        fpsDropdown.AddOptions(finalOptions);

        // Set the current FPS to the current FPS index
        string currentFPS = Application.targetFrameRate.ToString();
        if (currentFPS == "-1")
        {
            currentFPS = "Uncapped";
        }
        fpsDropdown.value = finalOptions.IndexOf(currentFPS);
        fpsDropdown.RefreshShownValue();
    }

    private void SetVsync(bool isVsync)
    {
        QualitySettings.vSyncCount = isVsync ? 1 : 0;
    }

    private void SetResolution(string resolutionString, bool isFullscreen)
    {
        // Parse the width and height from the resolution string
        string[] parts = resolutionString.Split('x');
        int width = int.Parse(parts[0]);
        int height = int.Parse(parts[1]);

        // Set the screen resolution
        Screen.SetResolution(width, height, isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }

    private void SetFPS(int fpsIndex)
    {
        Application.targetFrameRate = int.Parse(fpsDropdown.options[fpsIndex].text);
    }

    private void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void OnVsyncToggleValueChanged(bool isVsync)
    {
        //Disable the FPS dropdown if vsync is enabled
        fpsDropdown.interactable = !isVsync;
    }
}
