using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowHintOnTrigger : MonoBehaviour
{
    public GameObject hintPanel;
    public TMP_Text hintText;
    public Image hintImage;
    public Sprite frameSprite;

    public AudioSource audioSource;      // 🔊 The AudioSource component
    public AudioClip hintSoundEffect;    // 🎵 The sound to play

    private bool hasShownHint = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasShownHint) return;

        if (other.CompareTag("Player"))
        {
            hasShownHint = true;

            // ✅ Show UI
            hintPanel.SetActive(true);
            hintText.text = "You must use the hacking device on the frames";
            hintImage.sprite = frameSprite;

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
