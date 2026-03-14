using UnityEngine;

public class GoblinAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;        
    public Animator animator;       

    [Header("Detection")]
    public float attackRange = 3f;

    [Header("Wander")]
    public float walkSpeed = 1.2f;
    public float rotateSpeed = 8f;
    public float roamRadius = 6f;
    public float reachDistance = 0.3f;
    public float minIdleTime = 1f;
    public float maxIdleTime = 2.5f;

    [Header("Attack")]
    public int attackDamage = 10;
    public float attackCooldown = 1.2f;

    Health myHealth;
    Health playerHealth;

    Vector3 startPos;
    Vector3 roamTarget;
    float idleTimer;
    float nextAttackTime;

    void Start()
    {
        myHealth = GetComponent<Health>();
        startPos = transform.position;
        animator = GetComponentInChildren<Animator>();

        PickNewRoamTarget();
    }

    void Update()
    {
        if (myHealth.isDead)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
            AttackPlayer();
        else
            Wander();
    }

    void Wander()
    {
        if (idleTimer > 0f)
        {
            idleTimer -= Time.deltaTime;
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        Vector3 target = new Vector3(roamTarget.x, transform.position.y, roamTarget.z);
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        if (dir.magnitude < reachDistance)
        {
            idleTimer = Random.Range(minIdleTime, maxIdleTime);
            PickNewRoamTarget();
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        Vector3 moveDir = dir.normalized;

        transform.position += moveDir * walkSpeed * Time.deltaTime;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir) * Quaternion.Euler(0f, 180f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

        if (animator != null) animator.SetFloat("Speed", 1f); // walk
    }

    void PickNewRoamTarget()
    {
        Vector2 r = Random.insideUnitCircle * roamRadius;
        roamTarget = startPos + new Vector3(r.x, 0f, r.y);
    }

    void AttackPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized) * Quaternion.Euler(0f, 180f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

        if (animator != null) animator.SetFloat("Speed", 0f);

        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        if (animator != null) animator.SetTrigger("Attack");

        if (playerHealth == null)
            playerHealth = player.GetComponentInParent<Health>();

        if (playerHealth != null && !playerHealth.isDead)
            playerHealth.TakeDamage(attackDamage);
    }
}
