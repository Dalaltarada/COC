using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    private const string sceneKey = "SavedScene";
    private const string loadedFlag = "GameWasLoaded";

    public void LoadLastScene()
    {
        string sceneToLoad = PlayerPrefs.GetString(sceneKey, "");

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            PlayerPrefs.SetInt(loadedFlag, 1); 
            PlayerPrefs.Save();

            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No saved scene found!");
        }
    }
}
