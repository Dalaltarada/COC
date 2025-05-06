using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class DamagableEnemy : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 1f;

    private float currentHealth;

    [Tooltip("Event called when health drops to zero or below.")]
    public UnityEvent OnDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} health reached 0, invoking OnDeath");
            OnDeath?.Invoke();
        }
    }


    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        OnDeath?.Invoke();
    }
}
