using UnityEngine;
using UnityEngine.AI;

public class GoblinBehaviour : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    public float attackRange = 3f;

    public float walkSpeed = 1.2f;
    public float rotateSpeed = 8f;
    public float wanderRadius = 6f;
    public float minIdleTime = 1f;
    public float maxIdleTime = 2.5f;

    public int attackDamage = 10;
    public float attackCooldown = 1.2f;

    Health myHealth;
    Health playerHealth;
    NavMeshAgent agent;

    Vector3 startPosition;
    float idleTimeRemaining;
    float nextAttackTime;

    void Awake()
    {
        myHealth = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
    }

    void Start()
    {
        playerHealth = player.GetComponent<Health>();

        startPosition = transform.position;

        agent.speed = walkSpeed;

        SnapToNavMesh();

        if (agent.isOnNavMesh)
            PickNewWanderTarget();
    }

    void Update()
    {
        if (myHealth != null && myHealth.isDead)
        {
            if (agent.isOnNavMesh)
                agent.ResetPath();

            animator.SetFloat("Speed", 0f);
            
            return;
        }

        float distanceToPlayer;

        if (player != null)
            distanceToPlayer = Vector3.Distance(transform.position, player.position);
        else
            distanceToPlayer = Mathf.Infinity;

        if (distanceToPlayer <= attackRange)
            AttackPlayer();
        else
            Wander();

        UpdateFacing(distanceToPlayer);
        UpdateAnimation();
    }

    void SnapToNavMesh()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            agent.Warp(hit.position);
    }

    void Wander()
    {
        if (idleTimeRemaining > 0f)
        {
            idleTimeRemaining -= Time.deltaTime;

            if (idleTimeRemaining <= 0f)
                PickNewWanderTarget();

            return;
        }

        if (!agent.pathPending && (!agent.hasPath || agent.remainingDistance <= 0.15f))
        {
            idleTimeRemaining = Random.Range(minIdleTime, maxIdleTime);
            agent.ResetPath();
        }
    }

    void PickNewWanderTarget()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 random2D = Random.insideUnitCircle * wanderRadius;
            Vector3 candidate = startPosition + new Vector3(random2D.x, 0f, random2D.y);

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                agent.speed = walkSpeed;
                agent.SetDestination(hit.position);
                return;
            }
        }
    }

    void AttackPlayer()
    {
        if (agent.isOnNavMesh)
            agent.ResetPath();

        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackCooldown;

        animator.SetTrigger("Attack");

        if (playerHealth == null && player != null)
            playerHealth = player.GetComponent<Health>();

        if (playerHealth != null && !playerHealth.isDead)
            playerHealth.TakeDamage(attackDamage, DamageCause.Goblin);
    }

    void UpdateFacing(float distanceToPlayer)
    {
        Vector3 faceDirection = Vector3.zero;

        if (player != null && distanceToPlayer <= attackRange)
            faceDirection = player.position - transform.position;
        else if (agent.hasPath)
            faceDirection = agent.steeringTarget - transform.position;

        faceDirection.y = 0f;

        if (faceDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot =
                Quaternion.LookRotation(faceDirection.normalized) *
                Quaternion.Euler(0f, 180f, 0f);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }
    }

    void UpdateAnimation()
    {
        float horizontalSpeed = new Vector3(agent.velocity.x, 0f, agent.velocity.z).magnitude;
        float animSpeed = Mathf.Clamp01(horizontalSpeed / walkSpeed);
        animator.SetFloat("Speed", animSpeed);
    }
}