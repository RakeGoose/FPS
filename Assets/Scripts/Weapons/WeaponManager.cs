using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    public Weapon[] weapons;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadText;
    private Animator currentAnimator;
    private AudioSource audioSource;
    [SerializeField] private GameObject defaultImpactEffect;

    private bool isMeleeAttacking = false;

    private int currentWeaponIndex = 0;
    private float lastFireTime;
    private bool isReloading = false;

    private float reloadAnimTimer = 0f;
    private float reloadAnimInterval = 0.5f;
    private int reloadDotCount = 0;

    void Start()
    {
        foreach(var weapon in weapons)
        {
            weapon.currentAmmo = weapon.maxAmmo;
            weapon.totalAmmo = weapon.maxAmmo * 3;
        }

        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        EquipWeapon(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2);

        UpdateUI();
    }

    private Weapon CurrentWeapon() => weapons[currentWeaponIndex];
    int CurrentAmmo => CurrentWeapon().currentAmmo;
    int TotalAmmo => CurrentWeapon().totalAmmo;

    private void EquipWeapon(int index)
    {
        if (weapons[currentWeaponIndex].model != null)
            weapons[currentWeaponIndex].model.SetActive(false);

        currentWeaponIndex = index;
        currentAnimator = CurrentWeapon().model.GetComponent<Animator>();

        if (CurrentWeapon().model != null)
            CurrentWeapon().model.SetActive(true);

        UpdateUI();
    }

    private void Shoot()
    {

        if (CurrentWeapon().isMelee)
        {
            StartCoroutine(MeleeAttack());
            return;
        }

        var firePoint = CurrentWeapon().firePoint;

        if (CurrentWeapon().currentAmmo <= 0)
        {
            return;
        }

        if(CurrentWeapon().shootSound != null)
        {
            audioSource.PlayOneShot(CurrentWeapon().shootSound);
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

                if(CurrentWeapon().bloodEffect != null)
                {
                    Instantiate(CurrentWeapon().bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            else
            {
                if(defaultImpactEffect != null)
                {
                    GameObject impact = Instantiate(defaultImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 2f);
                }
            }
        }

        

        CurrentWeapon().currentAmmo--;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if(ammoText != null)
        {
            if (CurrentWeapon().isMelee)
            {
                ammoText.text = "∞";
            }
            else
            {
                ammoText.text = $"{CurrentWeapon().currentAmmo} / {CurrentWeapon().totalAmmo}";
            }
        }

        if (isReloading)
        {
            reloadAnimTimer += Time.deltaTime;
            if(reloadAnimTimer >= reloadAnimInterval)
            {
                reloadDotCount = (reloadDotCount + 1) % 4;
                string dots = new string('.', reloadDotCount);
                reloadText.text = $"Reloading{dots}";
                reloadAnimTimer = 0f;
            }

            reloadText.gameObject.SetActive(true);
        }
        else
        {
            reloadText.gameObject.SetActive(false);
            reloadAnimTimer = 0f;
            reloadDotCount = 0;
        }
    }

    public void TryShoot()
    {
        if (CurrentWeapon().isMelee)
        {
            if (!isMeleeAttacking)
            {
                StartCoroutine(MeleeAttack());
            }
            return;
        }

        if(Time.time >= lastFireTime + CurrentWeapon().fireRate && CurrentWeapon().currentAmmo > 0)
        {
            Shoot();
            lastFireTime = Time.time;
        }
    }

    public void TryReload()
    {
        if (isReloading || CurrentWeapon().currentAmmo == CurrentWeapon().maxAmmo || CurrentWeapon().totalAmmo <= 0)
            return;

        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        currentAnimator?.SetTrigger("Reload");

        yield return new WaitForSeconds(CurrentWeapon().reloadTime);

        int neededAmmo = CurrentWeapon().maxAmmo - CurrentWeapon().currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, CurrentWeapon().totalAmmo);

        CurrentWeapon().currentAmmo += ammoToReload;
        CurrentWeapon().totalAmmo -= ammoToReload;

        

        isReloading = false;

        UpdateUI();
    }

    IEnumerator MeleeAttack()
    {
        isMeleeAttacking = true;

        currentAnimator?.Play("KnifeAttack", 0, 0f);

        if(CurrentWeapon().meleeSwingSound != null)
        {
            audioSource.PlayOneShot(CurrentWeapon().meleeSwingSound);
        }
        else
        {
            Debug.LogWarning("meleeSwingSound is not assigned");
        }

        yield return new WaitForSeconds(CurrentWeapon().meleeDelay);
        

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, CurrentWeapon().meleeRange))
        {
            if(hit.collider.TryGetComponent(out EnemyLogic enemy))
            {
                enemy.TakeDamage(CurrentWeapon().damage);

                if (CurrentWeapon().bloodEffect != null)
                {
                    Instantiate(CurrentWeapon().bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if (CurrentWeapon().meleeHitSound != null)
                {
                    audioSource.PlayOneShot(CurrentWeapon().meleeHitSound);
                }
            }
            else
            {
                if (defaultImpactEffect != null)
                {
                    GameObject impact = Instantiate(defaultImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 2f);
                }
            }
        }

        yield return new WaitForSeconds(CurrentWeapon().fireRate);
        isMeleeAttacking = false;
    }
}
