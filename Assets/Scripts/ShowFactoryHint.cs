using UnityEngine;
using TMPro;

public class ShowFactoryHint : MonoBehaviour
{
    public GameObject hintPanel;
    public TMP_Text hintText;

    public AudioSource audioSource;      // 🔊 The AudioSource component
    public AudioClip hintSoundEffect;    // 🎵 The sound to play

    private bool hasShownHint = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasShownHint) return;

        if (other.CompareTag("Player"))
        {
            hasShownHint = true;

            // ✅ Show hint
            hintPanel.SetActive(true);
            hintText.text = "Items are found in the factory";

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
