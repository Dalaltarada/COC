using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneController : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene("Level1");
    }
}
