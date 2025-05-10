using UnityEngine;

public class CollectableLadder : Collectable
{
    [Header("Ladder Materials")]
    public Material material0;
    public Material material1;

    protected override void Update()
    {
        base.Update();

        // Optional: allow dropping the ladder manually
        if (PlayerInput.instance.mouse_left_click && is_in_players_pocket && gameObject.activeSelf)
        {
            PlayerCollectableManager.Instance.dropCollectable(this);
        }
    }

    public void UseLadderOnHint(ChangeLadderMaterial hintLadder)
    {
        if (hintLadder == null) return;

        // Apply materials to hint ladder
        hintLadder.ApplyMaterials(material0, material1);

        // Remove from inventory
        PlayerCollectableManager.Instance.removeCollectable(this);

        // Destroy this ladder (or you can SetActive(false) if you prefer)
        Destroy(gameObject);
    }
}
