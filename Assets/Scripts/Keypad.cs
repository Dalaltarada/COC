using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    public GameObject player;         // Reference to the player GameObject
    public GameObject keypadOB;       // Full keypad UI object

    public Text textOB;               // Text display for entered digits
    public string answer = "12345";   // Correct code

    public AudioSource button;        // Sound on keypad press
    public AudioSource correct;       // Sound on correct input
    public AudioSource wrong;         // Sound on wrong input

    void Start()
    {
        keypadOB.SetActive(false); // Hide keypad on start
    }

    public void Number(int number)
    {
        textOB.text += number.ToString();
        button.Play();
    }

    public void Execute()
    {
        if (textOB.text == answer)
        {
            correct.Play();
            textOB.text = "Success";
        }
        else
        {
            wrong.Play();
            textOB.text = "Wrong";
        }
    }

    public void Clear()
    {
        textOB.text = "";
        button.Play();
    }

    public void Exit()
    {
        keypadOB.SetActive(false);
        player.GetComponent<FirstPersonController>().canMove = true; // ✅ Re-enable movement
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (keypadOB.activeInHierarchy)
        {
            player.GetComponent<FirstPersonController>().canMove = false; // ⛔ Disable movement
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
