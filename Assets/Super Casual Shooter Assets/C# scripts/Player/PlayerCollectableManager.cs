using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectableManager : MonoBehaviour
{
    #region Singleton
    private static PlayerCollectableManager instance;
    public static PlayerCollectableManager Instance => instance;
    #endregion

    private List<Collectable> current_weapons = new List<Collectable>();
    public int weapon_at_hand_index { get; set; }
    public Transform hand;

    private PlayerInput player_input;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        player_input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (player_input != null && player_input.disinteract && current_weapons.Count > 0 && weapon_at_hand_index < current_weapons.Count)
        {
            dropCollectable(current_weapons[weapon_at_hand_index]);
        }

        updateWeaponIndex();
        updateWeaponAtHand();

        // ✅ Safely handle AimTexture and weapon logic
        if (AimTexture.Instance != null && current_weapons.Count > 0 && weapon_at_hand_index < current_weapons.Count)
        {
            Collectable currentWeapon = current_weapons[weapon_at_hand_index];
            if (currentWeapon != null && currentWeapon.GetType().BaseType == typeof(Gun))
            {
                AimTexture.Instance.setActive(true);
            }
            else
            {
                AimTexture.Instance.setActive(false);
            }
        }
        else if (AimTexture.Instance != null)
        {
            AimTexture.Instance.setActive(false);
        }
    }

    private void updateWeaponIndex()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            weapon_at_hand_index += 1;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            weapon_at_hand_index -= 1;
        }

        weapon_at_hand_index = Mathf.Clamp(weapon_at_hand_index, 0, current_weapons.Count - 1);
    }

    public void removeWeaponFromList(Collectable weapon)
    {
        current_weapons.Remove(weapon);
    }

    private void updateWeaponAtHand()
    {
        for (int i = 0; i < current_weapons.Count; i++)
        {
            if (weapon_at_hand_index == i)
            {
                current_weapons[i].gameObject.SetActive(true);
            }
            else
            {
                current_weapons[i].gameObject.SetActive(false);
            }
        }
    }

    public void assignCollectableToHand(Collectable collectable)
    {
        collectable.pickUpThisObject();

        Vector3 off_Set = collectable.offset;
        off_Set = hand.rotation * off_Set;
        collectable.GetComponent<Rigidbody>().isKinematic = true;
        collectable.transform.SetParent(hand.transform);
        collectable.transform.position = (hand.transform.position + off_Set);
        collectable.transform.rotation = hand.rotation;

        current_weapons.Add(collectable);
    }

    public void dropCollectable(Collectable collectable)
    {
        collectable.dropThisObject();
        collectable.transform.SetParent(null);
        collectable.GetComponent<Rigidbody>().isKinematic = false;
        collectable.GetComponent<Rigidbody>().AddForce(transform.forward * 200);

        StartCoroutine(changeCollectable());

        IEnumerator changeCollectable()
        {
            yield return new WaitForSeconds(1);
            current_weapons.Remove(collectable);
            if (weapon_at_hand_index > 0)
            {
                weapon_at_hand_index -= 1;
            }
        }
    }

    public Collectable getCurrentHeldCollectable()
    {
        if (current_weapons.Count == 0)
            return null;

        return current_weapons[weapon_at_hand_index];
    }

    public void removeCollectable(Collectable collectable)
    {
        if (current_weapons.Contains(collectable))
        {
            collectable.dropThisObject();
            collectable.transform.SetParent(null);
            collectable.GetComponent<Rigidbody>().isKinematic = false;

            current_weapons.Remove(collectable);

            if (weapon_at_hand_index >= current_weapons.Count)
            {
                weapon_at_hand_index = Mathf.Clamp(weapon_at_hand_index - 1, 0, current_weapons.Count - 1);
            }
        }
    }

    public void SaveInventory()
    {
        List<string> collectedItemTags = new List<string>();

        foreach (var item in current_weapons)
        {
            collectedItemTags.Add(item.gameObject.tag);
        }

        string joined = string.Join(",", collectedItemTags);
        PlayerPrefs.SetString("InventoryItems", joined);
        PlayerPrefs.Save();

        Debug.Log("💾 Inventory saved: " + joined);
    }

    public void RestoreInventoryFromSaved()
    {
        string data = PlayerPrefs.GetString("InventoryItems", "");
        if (string.IsNullOrEmpty(data))
            return;

        string[] itemTags = data.Split(',');

        foreach (string tag in itemTags)
        {
            GameObject item = GameObject.FindGameObjectWithTag(tag);
            if (item != null && item.TryGetComponent<Collectable>(out var collectable))
            {
                assignCollectableToHand(collectable);
            }
            else
            {
                Debug.LogWarning($"⚠ Could not find or assign item with tag: {tag}");
            }
        }

        Debug.Log("✅ Inventory restored.");
    }
}
