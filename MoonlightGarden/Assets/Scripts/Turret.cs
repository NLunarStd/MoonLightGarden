using UnityEngine;

public class Turret : Structure
{
    [Header("Turret Parameters")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;

    private float nextFireTime = 0f;

    protected override void Update()
    {
        base.Update();
        if (Time.time >= nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void FireProjectile()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}