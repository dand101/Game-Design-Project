using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

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
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player took damage");

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        OnDeath?.Invoke();
    }
}