using System;
using UnityEngine;
using Random = System.Random;

public class EnemyScript : MonoBehaviour
{
    public EnemyHealth Health;
    public EnemyBehaviour Behaviour;
    public EnemyHitResp HitResp;

    public GameObject PowerUpPrefab;

    public float removalDelay = 4f;

    //depression
    [SerializeField] private PlayerStats playerStats;

    [Header("On Death materials")] public Material material;

    private void Start()
    {
        Health.OnTakeDamage += HitResp.HitPain;
        Health.OnDeath += Die;
    }

    public event Action<GameObject> OnDeath;

    private void Die(Vector3 position)
    {
        Behaviour.StopMoving();
        HitResp.HandleDeath();

        FallOver();

        var outline = GetComponent<Outline>();
        outline.enabled = false;

        var renderer = GetComponent<Renderer>();
        if (renderer != null && material != null) renderer.material = material;

        var dissolveScript = GetComponent<U10PS_DissolveOverTime>();
        if (dissolveScript != null) dissolveScript.enabled = true;

        var powerUpChance = 0.5f; // 50% chance to spawn a powerup
        var rand = new Random();
        if (rand.NextDouble() <= powerUpChance)
        {
            // Randomly choose between health and ammo powerup
            var powerUp = Instantiate(PowerUpPrefab, transform.position, Quaternion.identity);
            Type[] powerUpTypes = { typeof(PowerUpHealth), typeof(PowerUpAmmoIncrease) };
            var index = rand.Next(0, powerUpTypes.Length);
            powerUp.AddComponent(powerUpTypes[index]);
        }


        playerStats.AddPoints(10);

        OnDeath?.Invoke(gameObject);
        Destroy(gameObject, removalDelay);
    }

    // just to make it fall. will be replaced with a proper animation
    private void FallOver()
    {
        var characterController = GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;

        var rb = gameObject.AddComponent<Rigidbody>();

        var forceDirection = -transform.forward;
        var forceMagnitude = 10f;
        rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
    }
}