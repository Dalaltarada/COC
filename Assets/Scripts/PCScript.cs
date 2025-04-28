using Fungus;
using UnityEngine;

public class PCInteract : MonoBehaviour
{
    public Flowchart flowchart;
    public string interactBlockName = "PClnteraction"; // Changed to match your block

    private void OnMouseDown()
    {
        // Additional check to ensure we're not in a dialog already
        if (flowchart != null && !flowchart.GetExecutingBlocks().Contains(flowchart.FindBlock(interactBlockName)))
        {
            flowchart.ExecuteBlock(interactBlockName);
        }
    }
}