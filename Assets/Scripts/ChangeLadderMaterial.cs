using UnityEngine;
using Fungus;

public class ChangeLadderMaterial : MonoBehaviour
{
    private Material[] originalMaterials;
    private MeshRenderer meshRenderer;
    private bool hasChanged = false;
    private bool playerIsNear = false;

    public Flowchart flowchart;                 // 🔹 Assign this in the Inspector
    public string triggerBlockName = "OnLadderFixed"; // 🔹 Name of the block to execute in Fungus

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null && meshRenderer.materials.Length > 0)
        {
            originalMaterials = meshRenderer.materials;
        }
    }

    void Update()
    {
        if (playerIsNear && !hasChanged && Input.GetKeyDown(KeyCode.E))
        {
            var heldItem = PlayerCollectableManager.Instance.getCurrentHeldCollectable();
            var ladder = heldItem as CollectableLadder;

            if (ladder != null)
            {
                ladder.UseLadderOnHint(this);
            }
        }
    }

    public void ApplyMaterials(Material mat0, Material mat1)
    {
        if (hasChanged || meshRenderer == null || mat0 == null || mat1 == null)
            return;

        Material[] currentMaterials = meshRenderer.materials;

        currentMaterials[0] = mat0;
        if (currentMaterials.Length > 1)
        {
            currentMaterials[1] = mat1;
        }

        meshRenderer.materials = currentMaterials;
        hasChanged = true;

        // ✅ Trigger the Fungus block
        if (flowchart != null && !string.IsNullOrEmpty(triggerBlockName))
        {
            flowchart.ExecuteBlock(triggerBlockName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
