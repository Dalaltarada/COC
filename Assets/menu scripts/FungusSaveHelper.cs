using UnityEngine;

public class FungusSaveHelper : MonoBehaviour
{
    public void SaveScoreOnly()
    {
        PlayerPrefs.SetInt("Score", ScoreManager.Instance.GetScore());
        PlayerPrefs.Save();
        Debug.Log("💾 Score saved via Fungus.");
    }
}
