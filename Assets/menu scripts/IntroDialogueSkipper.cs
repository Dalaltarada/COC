using UnityEngine;
using Fungus;

public class IntroDialogueSkipper : MonoBehaviour
{
    public Flowchart flowchart;

    void Start()
    {
        int wasLoaded = PlayerPrefs.GetInt("GameWasLoaded", 0);

        if (wasLoaded == 0 && flowchart != null)
        {
            // Trigger the StartIntro block manually
            flowchart.ExecuteBlock("StartIntro");
            Debug.Log("🎬 Starting intro dialogue (fresh game)");
        }
        else
        {
            Debug.Log("🎬 Skipping intro dialogue (loaded from save)");
        }
    }
}
