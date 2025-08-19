using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;


    public System.Action OnDeath;
    public System.Action<int, int> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke();

        //Destroy(gameObject);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

}
