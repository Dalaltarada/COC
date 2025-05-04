using UnityEngine;

public class HintActivator : MonoBehaviour
{
    public Light hintLight; // assign this in the Inspector
    private bool hintShown = false;

    void Update()
    {
        if (ScoreManager.Instance == null || ItemTracker.Instance == null) return;

        int score = ScoreManager.Instance.GetScore();
        int collectedCount = ItemTracker.Instance.GetTotalCollectedCount();

        if (!hintShown)
        {
            Debug.Log($"[HintActivator] Score: {score}, Items: {collectedCount}, Light On: {hintLight.enabled}");

            if (score >= 50 && collectedCount >= 1)
            {
                hintLight.enabled = true;
                hintShown = true;
                Debug.Log("✅ HINT LIGHT ACTIVATED!");
            }
        }
    }



}
