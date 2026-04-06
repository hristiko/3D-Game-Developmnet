using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public bool isPlayer = false;

    public int maxHealth = 100;
    public int currentHealth;

    public bool destroyOnDeath = true;

    [Header("Death animation for Goblin")]
    public bool playDeathAnimation = false;
    public Animator animator;
    public float destroyDelay = 2f;

    public bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void ApplyDamage(int damageAmount)
    {
        if (isDead) return;
        if (damageAmount <= 0) return;

        currentHealth -= damageAmount;

        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log(gameObject.name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;

            if (CompareTag("Goblin"))
            {
                CapsuleCollider capsule = GetComponent<CapsuleCollider>();
                capsule.enabled = false;

                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.ResetPath();
                    agent.enabled = false;
                }
            }

            if (isPlayer)
            {
                SceneManager.LoadScene("GameLost");
                return;
            }

            if (CompareTag("Goblin") && SceneManager.GetActiveScene().name == "Level3")
            {
                Level3WinChecker checker = FindObjectOfType<Level3WinChecker>();
                if (checker != null)
                    checker.RegisterGoblinKill();
            }

            if (playDeathAnimation && animator != null)
            {
                animator.SetTrigger("Die");
                Destroy(gameObject, destroyDelay);
            }
            else
            {
                if (destroyOnDeath)
                    Destroy(gameObject);
                else
                    gameObject.SetActive(false);
            }
        }
    }
}