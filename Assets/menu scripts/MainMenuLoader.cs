using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    private const string sceneKey = "SavedScene";

    public void LoadSavedGame()
    {
        if (PlayerPrefs.HasKey(sceneKey))
        {
            string sceneToLoad = PlayerPrefs.GetString(sceneKey);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("⚠ No saved game found!");
        }
    }
}
