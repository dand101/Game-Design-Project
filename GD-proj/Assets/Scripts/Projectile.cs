using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    public Transform firingPoint;

    [SerializeField]
    public float maxDistance = 50f;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, firingPoint.position);

        if (distance > maxDistance)
        {
            Destroy(gameObject);
        }

    }
}
