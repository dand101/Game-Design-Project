using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyShootingBehaviour : EnemyBehaviour
{
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;
    public float fireRate = 20f;
    public int gunDamage = 10;
    public TrailConfigScriptableObject trailConfig;
    public ShootConfigScriptableObject ShootConfig;
    public float strafeDistance = 5f;
    public float waypointRadius = 5f;
    public float separationDistance = 2f;
    public float encircleRadius = 10f;

    private NavMeshAgent agent;
    private Vector3 currentWaypoint;
    private bool isShooting;
    private UnityEngine.Pool.ObjectPool<TrailRenderer> trailPool;

    public override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerHealth = player.GetComponent<PlayerHealth>();

        // Initialize the trail pool
        trailPool = new UnityEngine.Pool.ObjectPool<TrailRenderer>(CreateTrail);
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Start()
    {
        StartCoroutine(UpdateMovement());
    }

    protected override IEnumerator UpdateMovement()
    {
        var waitForSeconds = new WaitForSeconds(0.1f);

        while (enabled)
        {
            if (IsPlayerRange() && IsPlayerVisible())
            {
                if (!isShooting)
                {
                    agent.isStopped = true;
                    isShooting = true;
                    StartShooting();
                }
            }
            else if (!IsPlayerRange() && !IsPlayerVisible())
            {
                EncircleAndSeparate();
            }
            else
            {
                if (isShooting)
                {
                    isShooting = false;
                    StopShooting();
                    agent.isStopped = false;
                }

                StrafeAroundPlayer();
            }

            yield return waitForSeconds;
        }
    }

    private void StrafeAroundPlayer()
    {
        var strafeDirection = Vector3.Cross(Vector3.up, player.position - transform.position).normalized;
        var strafeTarget = player.position + strafeDirection * strafeDistance;

        // Move left and right while strafing
        var leftStrafeTarget = player.position + strafeDirection * strafeDistance;
        var rightStrafeTarget = player.position - strafeDirection * strafeDistance;

        if (Random.value > 0.5f)
            agent.SetDestination(leftStrafeTarget);
        else
            agent.SetDestination(rightStrafeTarget);
    }

    private void EncircleAndSeparate()
    {
        var offset = (transform.position - player.position).normalized * encircleRadius;
        var circlePosition = player.position + offset;

        var separationForce = Vector3.zero;
        foreach (var enemy in FindObjectsOfType<EnemyShootingBehaviour>())
            if (enemy != this)
            {
                var toOther = transform.position - enemy.transform.position;
                if (toOther.sqrMagnitude < separationDistance * separationDistance)
                    separationForce += toOther.normalized / toOther.magnitude;
            }

        var targetPosition = circlePosition + separationForce * separationDistance;
        agent.SetDestination(targetPosition);
    }

    private void StartShooting()
    {
        var delay = 1f / fireRate;
        InvokeRepeating("Shoot", delay, delay);
    }

    private void StopShooting()
    {
        CancelInvoke("Shoot");
    }

    private void Shoot()
    {
        if (!agent.isActiveAndEnabled) return;

        transform.LookAt(player.position);

        var direction = transform.forward
                        + new Vector3(
                            Random.Range(
                                -ShootConfig.Spread.x,
                                ShootConfig.Spread.x
                            ),
                            Random.Range(
                                -ShootConfig.Spread.y,
                                ShootConfig.Spread.y
                            ),
                            Random.Range(
                                -ShootConfig.Spread.z,
                                ShootConfig.Spread.z
                            )
                        );

        direction.Normalize();
        var trailPosition =
            transform.position + direction.normalized * 1.5f;
        var trailObj = new GameObject("EnemyTrail");
        trailObj.transform.position = trailPosition;
        var trailRenderer = trailObj.AddComponent<TrailRenderer>();
        ConfigureTrailRenderer(trailRenderer);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, targetLayer))
        {
            StartCoroutine(ReleaseTrail(trailObj, trailRenderer, trailConfig.Duration, trailPosition, hit.point));
            //Debug.DrawRay(transform.position, direction, Color.red, 1f);
            if (hit.collider.name == "Player")
            {
                DealDamageToPlayer(gunDamage);
                SurfaceManager.Instance.HandleImpact(hit.point, hit.normal);
            }
            else if (hit.collider != null)
            {
                SurfaceManager.Instance.HandleImpact(hit.point, hit.normal);
            }
        }
    }

    private void ConfigureTrailRenderer(TrailRenderer trailRenderer)
    {
        trailRenderer.material = trailConfig.Material;
        trailRenderer.widthCurve = trailConfig.WidthCurve;
        trailRenderer.time = trailConfig.Duration;
        trailRenderer.colorGradient = trailConfig.Color;
        trailRenderer.minVertexDistance = trailConfig.MinVertexDistance;
    }

    private IEnumerator ReleaseTrail(GameObject trailObj, TrailRenderer trailRenderer, float duration,
        Vector3 startPoint, Vector3 endPoint)
    {
        var elapsedTime = 0f;

        trailRenderer.transform.position = startPoint;

        trailRenderer.emitting = true;

        while (elapsedTime < duration)
        {
            var t = elapsedTime / duration;

            trailRenderer.transform.position = Vector3.Lerp(startPoint, endPoint, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        trailRenderer.emitting = false;

        yield return null;

        trailPool.Release(trailRenderer);

        Destroy(trailObj);
    }

    protected override bool IsPlayerInRange()
    {
        var direction = player.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out var hit, attackRange, obstacleLayer)) return false;

        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    protected bool IsPlayerRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    protected bool IsPlayerVisible()
    {
        var direction = player.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out var hit, attackRange, obstacleLayer)) return false;

        return true;
    }

    private TrailRenderer CreateTrail()
    {
        var instance = new GameObject("EnemyTrail");
        instance.SetActive(false);
        var trailRenderer = instance.AddComponent<TrailRenderer>();
        trailRenderer.shadowCastingMode = ShadowCastingMode.Off;
        trailRenderer.receiveShadows = false;
        return trailRenderer;
    }
}