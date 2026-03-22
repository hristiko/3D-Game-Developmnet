using UnityEngine;
using UnityEngine.AI;

public class GoblinBehaviour : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    public float attackRange = 3f;

    public float walkSpeed = 1.2f;
    public float rotateSpeed = 8f;
    public float roamRadius = 6f;
    public float minIdleTime = 1f;
    public float maxIdleTime = 2.5f;

    public int attackDamage = 10;
    public float attackCooldown = 1.2f;

    Health myHealth;
    Health playerHealth;
    NavMeshAgent agent;

    Vector3 startPos;
    float idleTimer;
    float nextAttackTime;

    void Start()
    {
        myHealth = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (player != null)
            playerHealth = player.GetComponentInParent<Health>();

        startPos = transform.position;

        agent.speed = walkSpeed;
        agent.acceleration = 8f;
        agent.angularSpeed = 0f;
        agent.stoppingDistance = 0f;
        agent.updateRotation = false;

        SnapToNavMesh();

        if (agent.isOnNavMesh)
            PickNewRoamTarget();
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

        if (!agent.isOnNavMesh)
        {
            SnapToNavMesh();
            animator.SetFloat("Speed", 0f);
            return;
        }

        float dist = player != null ? Vector3.Distance(transform.position, player.position) : Mathf.Infinity;

        if (dist <= attackRange)
            AttackPlayer();
        else
            Wander();

        UpdateFacing();
        UpdateAnimation();
    }

    void SnapToNavMesh()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            agent.Warp(hit.position);
    }

    void Wander()
    {
        if (idleTimer > 0f)
        {
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0f)
                PickNewRoamTarget();

            return;
        }

        if (!agent.pathPending && (!agent.hasPath || agent.remainingDistance <= 0.15f))
        {
            idleTimer = Random.Range(minIdleTime, maxIdleTime);
            agent.ResetPath();
        }
    }

    void PickNewRoamTarget()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 random2D = Random.insideUnitCircle * roamRadius;
            Vector3 candidate = startPos + new Vector3(random2D.x, 0f, random2D.y);

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                agent.speed = walkSpeed;
                agent.stoppingDistance = 0f;
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

        if (playerHealth != null && !playerHealth.isDead)
            playerHealth.TakeDamage(attackDamage);
    }

    void UpdateFacing()
    {
        Vector3 faceDirection = Vector3.zero;

        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
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