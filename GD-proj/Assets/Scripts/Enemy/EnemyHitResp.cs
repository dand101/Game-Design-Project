using System;
using UnityEngine;

// no animations yet
[DisallowMultipleComponent]
public class EnemyHitResp : MonoBehaviour
{
    private EnemyHealth Health;
    //private Animator Animator;

    private void Awake()
    {
        //Animator animator = GetComponent<Animator>();
        Health = GetComponent<EnemyHealth>();
    }

    public void HitPain(int dmg)
    {
        if (Health.CurrentHealth != 0)
        {
            //Animator.SetTrigger("Hit");
            //Debug.Log("Au.......");
        }
    }


    public void HandleDeath()
    {
    }
}