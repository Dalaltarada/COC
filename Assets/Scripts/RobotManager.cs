using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public static RobotManager Instance;

    [SerializeField] private GameObject wallToDisable;
    [SerializeField] private GameObject hintPanel;

    [Header("Sound Effect")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hintSound;

    private int robotCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterRobot()
    {
        robotCount++;
    }

    public void RobotDied()
    {
        robotCount--;
        Debug.Log($"☠️ Robot destroyed! Remaining: {robotCount}");

        if (robotCount <= 0)
        {
            Debug.Log("✅ All robots destroyed!");

            if (wallToDisable != null)
                wallToDisable.SetActive(false);

            Invoke(nameof(ShowHint), 4f); // ⏱ Delay showing the hint
        }
    }

    private void ShowHint()
    {
        if (hintPanel != null)
            hintPanel.SetActive(true);

        if (audioSource != null && hintSound != null)
            audioSource.PlayOneShot(hintSound);
    }
}
