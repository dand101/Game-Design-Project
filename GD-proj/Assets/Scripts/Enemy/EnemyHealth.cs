using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    
    [Header("Only to be seen in the inspector")]
    [SerializeField] private int Health = 100;

    public int CurrentHealth
    {
        get => Health;
        private set => Health = value;
    }

    public int MaxHealth
    {
        get => maxHealth;
        private set => maxHealth = value;
    }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    private void OnEnable()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int dmg)
    {
        int dmgTaken = Mathf.Clamp(dmg, 0, CurrentHealth);

        CurrentHealth -= dmgTaken;

        if (dmg != 0)
        {
            OnTakeDamage?.Invoke(dmgTaken);
        }

        if (CurrentHealth == 0 && dmgTaken != 0)
        {
            OnDeath?.Invoke(transform.position);
        }
    }
}