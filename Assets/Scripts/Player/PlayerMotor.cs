using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isSprinting;

    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;


    private float maxStamina = 100f;
    private float staminaDrainRate = 25f;
    private float staminaRecoveryRate = 15f;
    private float currentStamina;

    public float GetStamina() => currentStamina;
    public float GetMaxStamina() => maxStamina;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
    }


    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        float currentSpeed = isSprinting && currentStamina > 0 ? sprintSpeed : walkSpeed;

        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);

        if(isSprinting && currentStamina > 0)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if(currentStamina < 0)
            {
                currentStamina = 0;
                isSprinting = false;
            }
        }
        else if(currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            if(currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
        
    }

    public void SetSprint(bool sprinting)
    {
        isSprinting = sprinting && currentStamina > 0;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
