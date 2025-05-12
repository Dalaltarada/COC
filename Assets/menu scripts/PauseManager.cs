using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public FirstPersonController playerController; // Drag your player here

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        if (playerController != null)
            playerController.SetControlLock(true); // ⛔ disables movement + unlocks cursor
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (playerController != null)
            playerController.SetControlLock(false); // ✅ enables movement + locks cursor
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Unpause before changing scenes
        SceneManager.LoadScene("MainMenu"); // Replace with the exact name of your main menu scene if different
    }
}
