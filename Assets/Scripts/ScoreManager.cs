using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;

    private int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Set default scores based on level
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Level3":
                SetScore(400); // 8 enemies x 50
                break;
            case "Level4":
            case "Levell5":
                SetScore(610); // final static score
                break;
        }

        UpdateScoreUI();
    }

    public void AddPoints(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public int GetScore()
    {
        return score;
    }
}
