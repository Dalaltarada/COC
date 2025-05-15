using Fungus;
using UnityEngine;

public class IntroDialogueSkipper : MonoBehaviour
{
    public Flowchart flowchart;

    void Start()
    {
        if (flowchart == null)
        {
            Debug.LogError("❌ Flowchart not assigned in IntroDialogueSkipper!");
            return;
        }

        if (flowchart.HasBlock("StartIntro"))
        {
            flowchart.ExecuteBlock("StartIntro");
        }
        else
        {
            Debug.LogWarning("⚠ Block 'StartIntro' not found in Flowchart: " + flowchart.name);
        }
    }

}
