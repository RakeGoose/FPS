using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float fireRate = 0.2f;
    public float damage = 20f;
    public LayerMask hitMask;
    public Transform firePoint;
    public GameObject muzzleFlashPrefab;

    private float lastFireTime = 0f;

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= lastFireTime + fireRate)
        {
            Shoot();
            lastFireTime = Time.time;
        }
    }

    private void Shoot()
    {
        if(muzzleFlashPrefab != null && firePoint != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(flash, 0.05f);
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        if(Physics.Raycast(ray, out RaycastHit hit, 500f, hitMask))
        {
            Debug.Log($"Hit {hit.collider.name}");

            EnemyLogic enemy = hit.collider.GetComponent<EnemyLogic>();
            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
