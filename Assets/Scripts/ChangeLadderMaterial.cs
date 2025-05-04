using UnityEngine;

public class ChangeLadderMaterial : MonoBehaviour
{
    // Assign these in the Inspector
    public Material newMaterialForElement0;
    public Material newMaterialForElement1;

    private Material[] originalMaterials;
    private MeshRenderer meshRenderer;
    private bool hasChanged = false;  // Renamed to better reflect its purpose

    void Start()
    {
        // Get the MeshRenderer component
        meshRenderer = GetComponent<MeshRenderer>();

        // Store the original materials
        if (meshRenderer != null && meshRenderer.materials.Length > 0)
        {
            originalMaterials = meshRenderer.materials;
        }
    }

    void OnMouseDown()
    {
        // Only execute if we haven't changed the materials yet
        if (hasChanged) return;

        if (meshRenderer == null || newMaterialForElement0 == null || newMaterialForElement1 == null)
            return;

        Material[] currentMaterials = meshRenderer.materials;

        // Apply new materials (no toggle back)
        currentMaterials[0] = newMaterialForElement0;
        if (currentMaterials.Length > 1)
        {
            currentMaterials[1] = newMaterialForElement1;
        }

        meshRenderer.materials = currentMaterials;
        hasChanged = true;  // Mark as changed so it won't execute again
    }
}