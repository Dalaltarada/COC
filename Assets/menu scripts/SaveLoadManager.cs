using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveLoadManager : MonoBehaviour
{
    public GameObject player;

    private const string sceneKey = "SavedScene";
    private const string posX = "PlayerPosX";
    private const string posY = "PlayerPosY";
    private const string posZ = "PlayerPosZ";
    private const string loadedFlag = "GameWasLoaded";
    public static SaveLoadManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // optional: makes it persist across scenes
        }
        else
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }

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
        PlayerPrefs.SetInt(loadedFlag, 0);

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

        PlayerPrefs.SetInt("Score", ScoreManager.Instance.GetScore());
        PlayerPrefs.Save();
        Debug.Log($"\ud83d\uddd2\ufe0f Saved: Scene={sceneName}, Pos={player?.transform.position}");
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey(sceneKey))
        {
            Debug.LogWarning("\u26a0 No saved game found.");
            return;
        }

        PlayerPrefs.SetInt(loadedFlag, 1);
        PlayerPrefs.Save();

        string sceneToLoad = PlayerPrefs.GetString(sceneKey);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu") return;
        if (PlayerPrefs.GetInt(loadedFlag, 0) == 0) return;
        if (!PlayerPrefs.HasKey(posX)) return;

        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
        if (foundPlayer != null)
        {
            float x = PlayerPrefs.GetFloat(posX);
            float y = PlayerPrefs.GetFloat(posY);
            float z = PlayerPrefs.GetFloat(posZ);
            foundPlayer.transform.position = new Vector3(x, y, z);
            Debug.Log($"\u2705 Loaded player to position: {foundPlayer.transform.position}");
        }

        if (ScoreManager.Instance != null)
        {
            int savedScore = PlayerPrefs.GetInt("Score", 0);
            ScoreManager.Instance.SetScore(savedScore);
        }

        StartCoroutine(ResetLoadedFlagNextFrame());
    }

    private IEnumerator ResetLoadedFlagNextFrame()
    {
        yield return null;
        PlayerPrefs.SetInt(loadedFlag, 0);
        PlayerPrefs.Save();
        Debug.Log("\ud83d\udd04 GameWasLoaded flag reset");
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(sceneKey);
        PlayerPrefs.DeleteKey(posX);
        PlayerPrefs.DeleteKey(posY);
        PlayerPrefs.DeleteKey(posZ);
        PlayerPrefs.DeleteKey(loadedFlag);
        PlayerPrefs.DeleteKey("Score");
        Debug.Log("\ud83d\uddd1\ufe0f Save data cleared.");
    }
}
