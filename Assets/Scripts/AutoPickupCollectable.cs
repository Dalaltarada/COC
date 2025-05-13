using UnityEngine;

public class AutoPickupCollectable : MonoBehaviour
{
    [SerializeField] private float pickupRange = 2.5f; // Distance threshold
    private Transform playerTransform;
    private Collectable collectable;

    private void Start()
    {
        collectable = GetComponent<Collectable>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("🚫 Player not found in scene!");
        }
    }

    private void Update()
    {
        if (playerTransform == null || collectable == null || collectable.is_in_players_pocket)
            return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= pickupRange)
        {
            PlayerCollectableManager.Instance.assignCollectableToHand(collectable);
        }
    }
}
