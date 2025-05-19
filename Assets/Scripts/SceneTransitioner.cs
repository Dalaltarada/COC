using Fungus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public string nextSceneName = "Level1";

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
