using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Death animation for Goblin")]
    public bool playDeathAnimation = false;
    public Animator animator;
    public float destroyDelay = 2f;

    public bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        //if (isDead) return;
        //if (damageAmount <= 0) return;

        currentHealth -= damageAmount;

        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log(gameObject.name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;

            if (playDeathAnimation && animator != null)
            {
                animator.SetTrigger("Die");
                Destroy(gameObject, destroyDelay);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}