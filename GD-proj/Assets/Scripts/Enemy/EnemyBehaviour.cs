using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    //private Animator Animator;
    protected NavMeshAgent Agent;

    protected Transform player;
    protected PlayerHealth playerHealth;

    public float attackRange = 5f;
    public int damageAmount = 10;

    public float attackSpeed = 1.5f;
    private float nextAttackTime;

    public GameObject slashEffectPrefab;

    public float attackCooldown = 2f;

    private float lastAttackTime = 0f;



    public virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerHealth = player.GetComponent<PlayerHealth>();
    }

    public virtual void Start()
    {
        //AAAAAAAAAAAAAA 2
        //Animator = GetComponent<Animator>();
        nextAttackTime = Time.time;

        StartCoroutine(UpdateMovement());
    }

    protected virtual IEnumerator UpdateMovement()
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

    public void Chase()
    {
        Agent.isStopped = false;

        //animator.SetBool(IsWalking, true);
        Agent.SetDestination(player.position);
    }

    public virtual void Attack()
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

                if (Time.time - lastAttackTime > attackCooldown)
                {
                    Quaternion slashRotation = Quaternion.LookRotation(attackDirection, Vector3.up);
                    Instantiate(slashEffectPrefab, transform.position, slashRotation);
                    lastAttackTime = Time.time;
                }
               
            }
        }
    }

    protected virtual void DealDamageToPlayer(int damageAmount = 10)
    {
        playerHealth.TakeDamage(damageAmount);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected virtual bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    public void StopMoving()
    {
        StopAllCoroutines();

        if (Agent != null)
        {
            Agent.isStopped = true;
            Agent.enabled = false;
        }
    }
}