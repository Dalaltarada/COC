using UnityEngine;

public class KeypadTrigger : MonoBehaviour
{
    public GameObject keypadOB;     // The full keypad UI
    public GameObject keypadText;   // "Press E to interact" prompt
    public GameObject player;       // Reference to the player

    private bool inReach = false;

    void Start()
    {
        keypadOB.SetActive(false);
        keypadText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = true;
            keypadText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = false;
            keypadText.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inReach)
        {
            keypadOB.SetActive(true);
            player.GetComponent<FirstPersonController>().canMove = false; // ⛔ Disable movement
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
