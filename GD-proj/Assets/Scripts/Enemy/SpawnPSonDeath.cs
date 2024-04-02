using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(IDamageable))]
public class SpawnPSonDeath : MonoBehaviour
{
    [SerializeField] public ParticleSystem DeathSystem;
    public IDamageable Damageable;

    private void Awake()
    {
        Damageable = GetComponent<IDamageable>();
    }

    private void OnEnable()
    {
        Damageable.OnDeath += Damageable_OnDeath;
    }

    private void Damageable_OnDeath(Vector3 position)
    {
        Instantiate(DeathSystem, position, Quaternion.identity);
    }
}