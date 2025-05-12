using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveLoadManager : MonoBehaviour
{
    public GameObject player; // Optional for gameplay scenes

    private const string sceneKey = "SavedScene";
    private const string posX = "PlayerPosX";
    private const string posY = "PlayerPosY";
    private const string posZ = "PlayerPosZ";
    private const string loadedFlag = "GameWasLoaded";

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SaveGame()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString(sceneKey, sceneName);
        PlayerPrefs.SetInt(loadedFlag, 0); // Mark as fresh start

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            Vector3 pos = player.transform.position;
            PlayerPrefs.SetFloat(posX, pos.x);
            PlayerPrefs.SetFloat(posY, pos.y);
            PlayerPrefs.SetFloat(posZ, pos.z);
        }

        PlayerPrefs.Save();
        Debug.Log($"💾 Saved: Scene={sceneName}, Pos={player?.transform.position}");
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey(sceneKey))
        {
            Debug.LogWarning("⚠ No saved game found.");
            return;
        }

        PlayerPrefs.SetInt(loadedFlag, 1); // Mark as loading from save
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        string sceneToLoad = PlayerPrefs.GetString(sceneKey);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu") return; // ✅ Skip processing if we're in the main menu

        if (!PlayerPrefs.HasKey(posX)) return;

        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
        if (foundPlayer != null)
        {
            float x = PlayerPrefs.GetFloat(posX);
            float y = PlayerPrefs.GetFloat(posY);
            float z = PlayerPrefs.GetFloat(posZ);

            foundPlayer.transform.position = new Vector3(x, y, z);
            Debug.Log($"✅ Loaded player to position: {foundPlayer.transform.position}");
        }
        else
        {
            Debug.LogWarning("⚠ Player object not found in loaded scene.");
        }

        StartCoroutine(ResetLoadedFlagNextFrame());
    }

    private IEnumerator ResetLoadedFlagNextFrame()
    {
        yield return null; // Wait one frame
        PlayerPrefs.SetInt(loadedFlag, 0);
        PlayerPrefs.Save();
        Debug.Log("🔄 GameWasLoaded flag reset");
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(sceneKey);
        PlayerPrefs.DeleteKey(posX);
        PlayerPrefs.DeleteKey(posY);
        PlayerPrefs.DeleteKey(posZ);
        PlayerPrefs.DeleteKey(loadedFlag);
        Debug.Log("🗑️ Save data cleared.");
    }
}
