using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        TakeDamage(damageAmount, DamageCause.Unknown);
    }

    public void TakeDamage(int damageAmount, DamageCause damageCause)
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

            if (isPlayer && damageCause == DamageCause.Goblin)
            {
                SceneManager.LoadScene("GameLost");
                return;
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