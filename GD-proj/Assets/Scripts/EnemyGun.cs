using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//not used
public class EnemyGun : MonoBehaviour
{
    [SerializeField]
    Transform firingPoint;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    float bulletSpeed = 10f;

    [SerializeField]
    float fireSpeed = 0.2f;

    //bruh moment
    public static EnemyGun instance;

    private float lastShotTime = 0f;

    public void Awake()
    {
        instance = this;
    }

    public void Shoot()
    {
        if ( fireSpeed + lastShotTime <= Time.time)
        {
            lastShotTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            Projectile projectile = bullet.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.firingPoint = firingPoint;
            }
            
            
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.AddForce(firingPoint.forward * bulletSpeed, ForceMode.Impulse);
        }
        
    }
}
