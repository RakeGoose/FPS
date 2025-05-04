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
    public GameObject muzzleFlashPrefab;
    public GameObject model;
    public Transform firePoint;
}
