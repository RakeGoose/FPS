using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions OnFoot;

    [SerializeField] private WeaponManager weaponManager;
    private bool isShooting = false;

    private PlayerMotor motor;
    private PlayerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        OnFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        OnFoot.Jump.performed += ctx => motor.Jump();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void FixedUpdate()
    {
        motor.ProcessMove(OnFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(OnFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        OnFoot.Enable();
        OnFoot.Shoot.started += ctx => isShooting = true;
        OnFoot.Shoot.canceled += ctx => isShooting = false;
        OnFoot.Reload.performed += ctx => weaponManager.TryReload();
        OnFoot.Sprint.performed += ctx => motor.SetSprint(true);
        OnFoot.Sprint.canceled += ctx => motor.SetSprint(false);
    }

    private void OnDisable()
    {
        OnFoot.Disable();
    }

    private void Update()
    {
        if (isShooting)
        {
            weaponManager.TryShoot();
        }
    }
}
