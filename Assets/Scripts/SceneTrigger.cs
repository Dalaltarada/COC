using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class SceneTrigger : MonoBehaviour
{
    public bool GuiOn;
    public string WindowTitle = "Go inside?";
    public Rect BoxSize = new Rect(0, 0, 200, 100);
    public GUISkin customSkin;

    private float displayEndTime = 0f;
    private AudioSource audioSource;

    [System.Serializable]
    public struct buttons
    {
        public string Name;
        public enum onPressFunction { CloseWindow, LoadAScene }
        public onPressFunction OnPressFunction;
        public Rect ButtonSize;
        public string LoadSceneName;

        public buttons(string name, onPressFunction function, Rect buttonSize, string sceneName)
        {
            Name = name;
            OnPressFunction = function;
            ButtonSize = buttonSize;
            LoadSceneName = sceneName;
        }
    }

    public buttons[] ButtonsOptions = new buttons[2]
    {
        new buttons("Enter", buttons.onPressFunction.LoadAScene, new Rect (10,60,80,30), "Inside"),
        new buttons("Not Yet", buttons.onPressFunction.CloseWindow, new Rect (110, 60,80,30), "")
    };

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter()
    {
        GuiOn = true;
        displayEndTime = Time.time + 5f;

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void OnGUI()
    {
        if (Time.time > displayEndTime)
        {
            GuiOn = false;
        }

        if (customSkin != null)
        {
            GUI.skin = customSkin;
        }

        if (GuiOn)
        {
            GUI.BeginGroup(new Rect((Screen.width - BoxSize.width) / 2, (Screen.height - BoxSize.height) / 2, BoxSize.width, BoxSize.height));
            GUI.Box(BoxSize, WindowTitle);

            for (int i = 0; i < ButtonsOptions.Length; ++i)
            {
                if (GUI.Button(ButtonsOptions[i].ButtonSize, ButtonsOptions[i].Name))
                {
                    if (ButtonsOptions[i].OnPressFunction == buttons.onPressFunction.CloseWindow)
                    {
                        GuiOn = false;
                    }
                    else if (ButtonsOptions[i].OnPressFunction == buttons.onPressFunction.LoadAScene)
                    {
                        SceneManager.LoadScene(ButtonsOptions[i].LoadSceneName, LoadSceneMode.Single);
                    }
                }
            }

            GUI.EndGroup();
        }
    }
}
