using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public delegate void DeathEvent();

    public delegate void TakeDamageEvent(int damage);

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject deathMenu; // Reference to the Death Menu UI

    [Header("Only to be seen in the inspector")]
    public int currentHealth = 100;

    private bool isDead;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = playerStats.C_Health;
        deathMenu.SetActive(false); // Ensure the death menu is hidden at the start
    }

    private void Update()
    {
        if (isDead && Input.GetKeyDown(KeyCode.Return)) // Check for Enter key press
            ReturnToMainMenu();
    }

    public event TakeDamageEvent OnTakeDamage;
    public event DeathEvent OnDeath;

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        playerStats.C_Health = currentHealth;

        OnTakeDamage?.Invoke(damage);

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Player died");
        isDead = true;

        // Show the death menu
        deathMenu.SetActive(true);

        OnDeath?.Invoke();
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene"); // Replace "MainMenuScene" with your main menu scene name
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        playerStats.C_Health = currentHealth;
    }
}