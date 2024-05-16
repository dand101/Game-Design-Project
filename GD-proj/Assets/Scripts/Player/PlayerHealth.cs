using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private PlayerStats playerStats;

    [Header("Only to be seen in the inspector")]
    public int currentHealth = 100;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public delegate void TakeDamageEvent(int damage);

    public event TakeDamageEvent OnTakeDamage;

    public delegate void DeathEvent();

    public event DeathEvent OnDeath;

    private void Start()
    {
        currentHealth = playerStats.C_Health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // cursed
        playerStats.C_Health = currentHealth;

        OnTakeDamage?.Invoke(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died");

        // Restart when dead
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        OnDeath?.Invoke();
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // cursed
        playerStats.C_Health = currentHealth;
    }
}