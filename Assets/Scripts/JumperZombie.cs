using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperZombie : EnemyLogic
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    protected override void AttackPlayer()
    {
        if(Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            Vector3 jumpDir = (player.position - transform.position).normalized;
            rb.AddForce(jumpDir * jumpForce + Vector3.up * jumpForce, ForceMode.Impulse);

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(15f);
            }
        }

        if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = EnemyState.Following;
        }
    }
}
