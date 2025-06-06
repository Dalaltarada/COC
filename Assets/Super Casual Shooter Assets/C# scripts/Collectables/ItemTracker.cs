using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemTracker : MonoBehaviour
{
    public static ItemTracker Instance;

    [Header("UI References")]
    public TMP_Text ak47Text;
    public TMP_Text bombControllerText;
    public TMP_Text grenadeText;

    private Dictionary<string, int> collectedItems = new Dictionary<string, int>();
    private Dictionary<string, int> requiredItems = new Dictionary<string, int>()
    {
        { "AK47", 1 },
        { "BombController", 1 },
        { "Grenade", 4 }
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Initialize collected items
        foreach (var item in requiredItems)
            collectedItems[item.Key] = 0;

        UpdateUI();
    }

    public void CollectItem(string itemName)
    {
        if (collectedItems.ContainsKey(itemName))
        {
            collectedItems[itemName]++;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        ak47Text.text = $"AK47 Gun {collectedItems["AK47"]}/{requiredItems["AK47"]}";
        bombControllerText.text = $"Hacking Device {collectedItems["BombController"]}/{requiredItems["BombController"]}";
        grenadeText.text = $"Grenade {collectedItems["Grenade"]}/{requiredItems["Grenade"]}";
    }

    public bool AllItemsCollected()
    {
        foreach (var pair in collectedItems)
        {
            if (pair.Value < requiredItems[pair.Key])
                return false;
        }
        return true;
    }

    // For testing
    public List<int> GetCollectedItemCounts()
    {
        return new List<int>(collectedItems.Values);
    }

    public int GetTotalCollectedCount()
    {
        int total = 0;
        foreach (var item in collectedItems.Values)
        {
            total += item;
        }
        return total;
    }
}
