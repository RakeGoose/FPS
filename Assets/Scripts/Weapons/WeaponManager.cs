using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    public Weapon[] weapons;
    public TextMeshProUGUI ammoText;

    private int currentWeaponIndex = 0;
    private int currentAmmo;
    private float lastFireTime;

    void Start()
    {
        EquipWeapon(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2);

    }

    private Weapon CurrentWeapon() => weapons[currentWeaponIndex];

    private void EquipWeapon(int index)
    {
        foreach (var weapon in weapons)
            weapon.model.SetActive(false);
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].model.SetActive(true);
        currentAmmo = weapons[currentWeaponIndex].maxAmmo;
        UpdateUI();
    }

    private void Shoot()
    {
        var firePoint = CurrentWeapon().firePoint;

        if (currentAmmo <= 0)
        {
            return;
        }

        if(CurrentWeapon().muzzleFlashPrefab != null)
        {
            var flash = Instantiate(CurrentWeapon().muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(flash, 0.05f);
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        if(Physics.Raycast(ray, out RaycastHit hit, 500f))
        {
            if(hit.collider.TryGetComponent(out EnemyLogic enemy))
            {
                enemy.TakeDamage(CurrentWeapon().damage);
            }
        }

        currentAmmo--;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if(ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {CurrentWeapon().maxAmmo}";
        }
    }

    public void TryShoot()
    {
        if(Time.time >= lastFireTime + CurrentWeapon().fireRate && currentAmmo > 0)
        {
            Shoot();
            lastFireTime = Time.time;
        }
    }
}
