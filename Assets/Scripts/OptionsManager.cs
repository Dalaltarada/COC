using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public Dropdown resolutionDropdown;
    public GameObject controlsPanel;
    public GameObject optionsPanel;
    public GameObject mainMenuPanel;
    public GameObject instructionsPanel;

    private Resolution[] resolutions;

    void Start()
    {
        // 🎧 Volume setup
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.8f);
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        SetVolume(savedVolume);

        // 🎮 Sensitivity setup
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveAllListeners();
            sensitivitySlider.value = savedSensitivity;
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }
        SetSensitivity(savedSensitivity);

        // 🖥️ Resolution setup
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
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        SetResolution(savedResIndex);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        Debug.Log("🔊 Volume set to: " + volume);
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        Debug.Log("🎮 Sensitivity set to: " + value);
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", index);
        Debug.Log("🖥️ Resolution set to: " + res.width + " x " + res.height);
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void BackToPreviousMenu()
    {
        Debug.Log("🔙 BackToPreviousMenu clicked");
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void BackFromControls()
    {
        controlsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void BackFromInstructions()
    {
        instructionsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
}
