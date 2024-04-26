using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Damage Config", menuName = "Guns/DamageConfig", order = 5)]
public class DamageConfigScriptableObject : ScriptableObject
{
    // pain
    public MinMaxCurve dmgCurve;

    private void Reset()
    {
        dmgCurve.mode = ParticleSystemCurveMode.Curve;
    }

    public int GetDamage(float distance = 0)
    {
        return Mathf.CeilToInt(dmgCurve.Evaluate(distance, Random.value));
    }
}