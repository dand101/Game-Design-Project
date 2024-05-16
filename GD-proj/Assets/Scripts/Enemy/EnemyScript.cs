using System;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemyScript : MonoBehaviour
{
    public EnemyHealth Health;
    public EnemyBehaviour Behaviour;
    public EnemyHitResp HitResp;

    public GameObject PowerUpPrefab;

    public float removalDelay = 4f;
    public event Action<GameObject> OnDeath;

    //depression
    [SerializeField] private PlayerStats playerStats;

    [Header("On Death materials")] public Material material;


    private void Start()
    {
        Health.OnTakeDamage += HitResp.HitPain;
        Health.OnDeath += Die;
    }

    private void Die(Vector3 position)
    {
        Behaviour.StopMoving();
        HitResp.HandleDeath();

        FallOver();

        Outline outline = GetComponent<Outline>();
        outline.enabled = false;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && material != null)
        {
            renderer.material = material;
        }

        U10PS_DissolveOverTime dissolveScript = GetComponent<U10PS_DissolveOverTime>();
        if (dissolveScript != null)
        {
            dissolveScript.enabled = true;
        }

        float powerUpChance = 0.5f; // 50% chance to spawn a powerup
        System.Random rand = new System.Random();
        if (rand.NextDouble() <= powerUpChance)
        {
            // Randomly choose between health and ammo powerup
            GameObject powerUp = Instantiate(PowerUpPrefab, transform.position, Quaternion.identity);
            Type[] powerUpTypes = { typeof(PowerUpHealth), typeof(PowerUpAmmoIncrease) };
            int index = rand.Next(0, powerUpTypes.Length);
            powerUp.AddComponent(powerUpTypes[index]);
        }


        playerStats.AddPoints(10);

        OnDeath?.Invoke(gameObject);
        Destroy(gameObject, removalDelay);
    }

    // just to make it fall. will be replaced with a proper animation
    private void FallOver()
    {
        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();

        Vector3 forceDirection = -transform.forward;
        float forceMagnitude = 10f;
        rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
    }
}