using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceManager : MonoBehaviour
{
    public static SurfaceManager Instance;

    [Tooltip("The impact effect prefab to be played on surface hits.")]
    public GameObject impactEffectPrefab;

    [Tooltip("The muzzle effect prefab to be played when shooting.")]
    public GameObject muzzleEffectPrefab;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one SurfaceManager active in the scene! Destroying latest one: " + name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void HandleImpact(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (impactEffectPrefab != null)
        {
            GameObject impactEffect = Instantiate(impactEffectPrefab, hitPoint, Quaternion.LookRotation(hitNormal));

            // impactEffect.transform.localScale = new Vector3(1, 1, 1);
            // impactEffect.GetComponent<ParticleSystem>().Stop(); // if it's a particle system
            // impactEffect.GetComponent<ParticleSystem>().Play();

            float effectDuration = 0f;
            ParticleSystem particleSystem = impactEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                effectDuration = particleSystem.main.duration;
            }
            else
            {
                effectDuration = 2.0f;
            }

            Destroy(impactEffect, effectDuration);
        }
        else
        {
            Debug.LogWarning("No impact effect prefab assigned to SurfaceManager. Please assign one in the inspector.");
        }
    }

    public void PlayMuzzleEffect(GameObject parent, Vector3 position, Quaternion rotation)
    {
        if (muzzleEffectPrefab != null)
        {
            GameObject muzzleEffectInstance = Instantiate(muzzleEffectPrefab, position, rotation, parent.transform);

            ParticleSystem muzzleParticleSystem = muzzleEffectInstance.GetComponent<ParticleSystem>();

            if (muzzleParticleSystem != null)
            {
                float effectDuration = muzzleParticleSystem.main.duration;
                Destroy(muzzleEffectInstance, effectDuration);
            }
            else
            {
                Debug.LogWarning("No ParticleSystem component found in the muzzle effect prefab.");
            }
        }
        else
        {
            Debug.LogWarning("No muzzle effect prefab assigned to SurfaceManager. Please assign one in the inspector.");
        }
    }
}