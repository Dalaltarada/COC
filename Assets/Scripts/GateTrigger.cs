using UnityEngine;
using Fungus;

public class GateTrigger : MonoBehaviour
{
    [SerializeField] private Flowchart flowchart;
    [SerializeField] private string blockName = "GoToLevel4";

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (flowchart != null)
        {
            flowchart.ExecuteBlock(blockName);
            Debug.Log("🎬 Triggered Fungus block: " + blockName);
        }
    }
}
