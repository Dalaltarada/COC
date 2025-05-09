using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public float slideDistance = 2f;
    public float slideSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isPlayerNear = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + transform.forward * slideDistance;

    }

    void Update()
    {
        Vector3 targetPosition = isPlayerNear ? openPosition : closedPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, slideSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
