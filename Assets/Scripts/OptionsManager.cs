using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public Dropdown resolutionDropdown;
    public GameObject controlsPanel;
    public GameObject optionsPanel;

    private Resolution[] resolutions;

    void Start()
    {
        // Load & apply saved volume
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        // Load & apply saved sensitivity
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        sensitivitySlider.value = savedSensitivity;

        // Setup resolution options
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int currentResolutionIndex = 0;
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        int savedResIndex = PlayerPrefs.GetInt("Resolution", currentResolutionIndex);
        resolutionDropdown.value = savedResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        // Example: Your player script can read from PlayerPrefs.GetFloat("Sensitivity")
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", index);
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void BackToPreviousMenu()
    {
        optionsPanel.SetActive(false);
    }

    public void BackFromControls()
    {
        controlsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
}
