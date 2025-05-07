using UnityEngine;
using UnityEngine.Events;

public class RobotDamagable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 1f;
    private float currentHealth;
    private bool isDead = false;

    public UnityEvent OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        Debug.Log($"{gameObject.name} spawned with health: {currentHealth}");
    }

    public void TakeDamage(float amount)
    {
        Debug.Log($"🛠️ TakeDamage called on {gameObject.name} for {amount}");

        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            isDead = true;
            currentHealth = 0;
            Debug.Log($"{gameObject.name} has died.");
            OnDeath?.Invoke();
        }
    }

    private void Update()
    {
        // Press L to damage manually
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(0.3f);
        }
    }

    // ✅ Respond to mouse click (anywhere on collider)
    private void OnMouseDown()
    {
        TakeDamage(0.3f); // or any amount you want
        Debug.Log("🖱️ Robot clicked! Damage applied.");
    }
}
