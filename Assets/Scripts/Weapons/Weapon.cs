using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public float fireRate;
    public float damage;

    public int maxAmmo;
    public int currentAmmo;
    public int totalAmmo;
    public float reloadTime = 2f;

    public GameObject muzzleFlashPrefab;
    public GameObject model;
    public Transform firePoint;

    public bool isMelee;
    public float meleeRange;
    public float meleeDelay;
}
