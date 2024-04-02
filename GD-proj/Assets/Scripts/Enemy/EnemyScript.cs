using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyHealth Health;
    public EnemyBehaviour Behaviour;
    public EnemyHitResp HitResp;

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