using UnityEngine;
using TMPro; // ✅ TextMeshPro namespace

public class HintDisplay : MonoBehaviour
{
    public TextMeshProUGUI hintText;     // Reference to the TMP UI Text
    public string hintMessage = "0003";  // The hint message to display
    public float displayDuration = 5f;   // Time in seconds to show the hint

    private bool isDisplayed = false;

    private void OnMouseDown()
    {
        if (!isDisplayed)
        {
            StartCoroutine(ShowHint());
        }
    }

    private System.Collections.IEnumerator ShowHint()
    {
        isDisplayed = true;
        hintText.text = hintMessage;
        hintText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        hintText.text = "";
        hintText.gameObject.SetActive(false);
        isDisplayed = false;
    }
}
