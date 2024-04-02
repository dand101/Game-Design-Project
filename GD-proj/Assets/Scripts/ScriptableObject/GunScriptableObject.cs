using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    [Header("References")] public GunType Type;
    public string Name;

    public GameObject ModelPrefab;

    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;


    public ShootConfigScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;


    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;

    private float LastShootTime;

    private ParticleSystem ShootSystem;

    private ObjectPool<TrailRenderer> TrailPool;

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
    }

    public void Shoot()
    {
        if (Time.time > ShootConfig.FireRate + LastShootTime)
        {
            LastShootTime = Time.time;
            ShootSystem.Play();
            var shootDirection = ShootSystem.transform.forward
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
            shootDirection.Normalize();

            if (Physics.Raycast(
                    ShootSystem.transform.position,
                    shootDirection,
                    out var hit,
                    float.MaxValue,
                    ShootConfig.HitMask
                ))
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        ShootSystem.transform.position,
                        hit.point,
                        hit
                    )
                );
                var muzzlePosition = ShootSystem.transform.position + ShootSystem.transform.forward * 0.5f;
                var muzzleRotation = ShootSystem.transform.rotation;
                SurfaceManager.Instance.PlayMuzzleEffect(muzzlePosition, muzzleRotation);
            }
            else
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        ShootSystem.transform.position,
                        ShootSystem.transform.position + shootDirection * TrailConfig.MissDistance,
                        new RaycastHit()
                    )
                );
                var muzzlePosition = ShootSystem.transform.position + ShootSystem.transform.forward * 0.5f;
                var muzzleRotation = ShootSystem.transform.rotation;
                SurfaceManager.Instance.PlayMuzzleEffect(muzzlePosition, muzzleRotation);
            }
        }
    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    {
        var instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null;

        instance.emitting = true;

        var distance = Vector3.Distance(StartPoint, EndPoint);
        var remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - remainingDistance / distance)
            );
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;


        if (Hit.collider != null)
        {
            SurfaceManager.Instance.HandleImpact(Hit.point, Hit.normal);
            
            // commented out uncomment later lol
            // // trigger the collision event
            // Hit.collider.gameObject.SendMessage("OnCollisionEnter", new Collision(), SendMessageOptions.DontRequireReceiver);
        }

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        var instance = new GameObject("Bullet Trail");
        var trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}