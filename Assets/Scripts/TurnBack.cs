using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TurnBack : MonoBehaviour
{
    public bool GuiOn;
    public string Text = "Turn Back";
    public Rect BoxSize = new Rect(0, 0, 200, 100);
    public GUISkin customSkin;

    private float displayEndTime = 0f;

    void OnTriggerEnter()
    {
        GuiOn = true;
        displayEndTime = Time.time + 5f; // Show for 5 seconds
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
            GUI.Label(BoxSize, Text);
            GUI.EndGroup();
        }
    }
}
