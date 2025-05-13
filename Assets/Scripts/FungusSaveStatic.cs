using UnityEngine;

public class FungusSaveStatic : MonoBehaviour
{
    public void SaveGameDirectly()
    {
        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.SaveGame();
            Debug.Log("\ud83d\uddd2\ufe0f Save triggered via static instance from Fungus.");
        }
        else
        {
            Debug.LogWarning("\u26a0 SaveLoadManager.Instance is null.");
        }
    }
}
