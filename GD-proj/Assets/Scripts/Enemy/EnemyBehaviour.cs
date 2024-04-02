using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    //no animator yet AAAAAAAAAAAAAAAAA
    //private Animator Animator;
    private NavMeshAgent Agent;

    private Transform player;
    private PlayerHealth playerHealth;

    public float attackRange = 5f;
    public int damageAmount = 10;

    public float attackSpeed = 1.5f;
    private float nextAttackTime;


    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        //AAAAAAAAAAAAAA 2
        //Animator = GetComponent<Animator>();
        nextAttackTime = Time.time;

        StartCoroutine(UpdateMovement());
    }

    private IEnumerator UpdateMovement()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while (enabled)
        {
            if (IsPlayerInRange() && Time.time >= nextAttackTime)
            {
                //Debug.Log("Attacking player");
                Attack();
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
            else if (!IsPlayerInRange())
            {
                //Debug.Log("Chasing player");
                Chase();
            }

            yield return waitForSeconds;
        }
    }

    private void Chase()
    {
        //Debug.Log("Chasing player");
        Agent.isStopped = false;

        //animator.SetBool(IsWalking, true);
        Agent.SetDestination(player.position);
    }

    private void Attack()
    {
        Agent.isStopped = true;

        Vector3 attackDirection = player.position - transform.position;
        float distanceToPlayer = attackDirection.magnitude;
        attackDirection.Normalize();

        if (distanceToPlayer <= attackRange)
        {
            transform.LookAt(player);

            float angle = Vector3.Angle(transform.forward, attackDirection);

            if (angle < 45f)
            {
                Debug.DrawRay(transform.position, attackDirection * attackRange, Color.green, 0.1f);
                DealDamageToPlayer();
            }
        }
    }

    private void DealDamageToPlayer()
    {
        playerHealth.TakeDamage(damageAmount);
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    public void StopMoving()
    {
        StopAllCoroutines();
        Agent.isStopped = true;
        Agent.enabled = false;
    }
}