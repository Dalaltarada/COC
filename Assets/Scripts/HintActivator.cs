using System.Collections;
using UnityEngine;
using TMPro;

public class HintActivator : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    public GameObject hintTargetBox; // ✅ Add this
    private bool hintShown = false;

    void Update()
    {
        if (hintShown || ScoreManager.Instance == null || ItemTracker.Instance == null)
            return;

        bool allItemsCollected = ItemTracker.Instance.AllItemsCollected();
        bool allDronesDead = AllDronesDead();

        if (allItemsCollected && allDronesDead)
        {
            StartCoroutine(ShowHintTemporary());
            hintTargetBox.SetActive(true); // ✅ Show green box
            hintShown = true;
            Debug.Log("✅ HINT MESSAGE & BOX DISPLAYED!");
        }
    }

    private bool AllDronesDead()
    {
        return GameObject.FindObjectsOfType<EnemyDrone>().Length == 0;
    }

    private IEnumerator ShowHintTemporary()
    {
        hintText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        hintText.gameObject.SetActive(false);
    }
}
