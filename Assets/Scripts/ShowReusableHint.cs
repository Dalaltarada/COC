using UnityEngine;
using TMPro;

public class ShowReusableHint : MonoBehaviour
{
    [Header("UI")]
    public GameObject hintPanel;
    public TMP_Text hintText;

    [Header("Hint Settings")]
    [TextArea]
    public string hintMessage = "Default Hint Message";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hintSoundEffect;

    private bool hasShownHint = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasShownHint) return;

        if (other.CompareTag("Player"))
        {
            hasShownHint = true;

            // ✅ Show hint
            hintPanel.SetActive(true);
            hintText.text = hintMessage;

            // 🔊 Play sound
            if (audioSource != null && hintSoundEffect != null)
            {
                audioSource.PlayOneShot(hintSoundEffect);
            }

            Invoke(nameof(HideHint), 5f);
        }
    }

    private void HideHint()
    {
        hintPanel.SetActive(false);
    }
}
