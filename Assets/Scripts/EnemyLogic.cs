using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Following,
        Searching,
        Attacking
    }

    public enum EnemyType
    {
        Walker,
        Jumper
    }

    [Header("General Settings")]
    public EnemyState currentState = EnemyState.Patrolling;
    public Transform[] patrolPoints;
    public float viewRadius = 10f;
    public float viewAngle = 120f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    [Header("AttackSettings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    protected float lastAttackTime = 0f;

    protected NavMeshAgent agent;
    protected Transform player;
    protected Vector3 lastKnownPosition;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                LookForPlayer();
                break;

            case EnemyState.Following:
                FollowPlayer();
                break;

            case EnemyState.Searching:
                SearchForPlayer();
                break;

            case EnemyState.Attacking:
                AttackPlayer();
                break;
        }
    }

    protected virtual void Patrol()
    {
        if(patrolPoints.Length == 0)
        {
            return;
        }

        if(!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Length)].position);
        }
    }

    protected virtual void LookForPlayer()
    {
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        foreach (var target in targetsInView)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if(!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
                {
                    currentState = EnemyState.Following;
                    lastKnownPosition = player.position;
                    break;
                }
            }
        }
    }

    protected virtual void FollowPlayer()
    {
        if (CanSeePlayer())
        {
            lastKnownPosition = player.position;
            agent.SetDestination(player.position);

            if(Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                currentState = EnemyState.Attacking;
            }
            else
            {
                currentState = EnemyState.Searching;
            }
        }
    }

    protected virtual void SearchForPlayer()
    {
        agent.SetDestination(lastKnownPosition);

        if(Vector3.Distance(transform.position, lastKnownPosition) <= 0.5f)
        {
            currentState = EnemyState.Patrolling;
        }

        LookForPlayer();
    }

    protected virtual void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        if(Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(10f);
            }
        }

        if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = EnemyState.Following;
        }
    }

    protected bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
        {
            if(!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
            {
                return true;
            }
        }

        return false;
    }
}
