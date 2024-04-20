using System;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemyScript : MonoBehaviour
{
    public EnemyHealth Health;
    public EnemyBehaviour Behaviour;
    public EnemyHitResp HitResp;

    public GameObject PowerUpPrefab;
    
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

        // spawn a PowerUp
        GameObject powerUp = Instantiate(PowerUpPrefab, transform.position, Quaternion.identity);

        // attach a random type to the PowerUp
        Type[] powerUpTypes = { typeof(PowerUpFireRate), typeof(PowerUpSpread), typeof(PowerUpHealth), typeof(PowerUpAmmoCapacity) };

        var rand = new System.Random();
        int index = rand.Next(0, powerUpTypes.Length);
        powerUp.AddComponent(powerUpTypes[index]);
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