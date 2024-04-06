using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

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

    private bool isShooting = false;
    private ObjectPool<TrailRenderer> trailPool;
    private NavMeshAgent agent;

    public override void Awake()
    {
        base.Awake();
        base.player = GameObject.FindGameObjectWithTag("Player").transform;

        playerHealth = player.GetComponent<PlayerHealth>();

        // Initialize the trail pool
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Start()
    {
        StartCoroutine(UpdateMovement());
    }

    protected override IEnumerator UpdateMovement()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while (enabled)
        {
            if (IsPlayerInRange())
            {
                if (!isShooting)
                {
                    Agent.isStopped = true;
                    isShooting = true;
                    StartShooting();
                }
            }
            else
            {
                if (isShooting)
                {
                    isShooting = false;
                    StopShooting();
                    Agent.isStopped = false;
                }

                Chase();
            }

            yield return waitForSeconds;
        }
    }

    private void StartShooting()
    {
        float delay = 1f / fireRate;
        InvokeRepeating("Shoot", delay, delay);
    }

    private void StopShooting()
    {
        CancelInvoke("Shoot");
    }

    private void Shoot()
    {
        if (!Agent.isActiveAndEnabled) return;

        transform.LookAt(player.position);

        Vector3 direction = transform.forward
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
        Vector3 trailPosition =
            transform.position + direction.normalized * 1.5f;
        GameObject trailObj = new GameObject("EnemyTrail");
        trailObj.transform.position = trailPosition;
        TrailRenderer trailRenderer = trailObj.AddComponent<TrailRenderer>();
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
        float elapsedTime = 0f;

        trailRenderer.transform.position = startPoint;

        trailRenderer.emitting = true;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

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
        Vector3 direction = player.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, attackRange, obstacleLayer))
        {
            return false;
        }

        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("EnemyTrail");
        instance.SetActive(false);
        TrailRenderer trailRenderer = instance.AddComponent<TrailRenderer>();
        trailRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        trailRenderer.receiveShadows = false;
        return trailRenderer;
    }
}
