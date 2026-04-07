using UnityEngine;

public class Mineral : MonoBehaviour
{
    public SphereCollider mineralHitZone;
    public int maxHealth = 3;
    public int materialReward = 1;

    int currentHealth;
    bool mined;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, PlayerInventory inventory)
    {
        if (mined) return;

        currentHealth -= amount;

        if (currentHealth < 0)
            currentHealth = 0;

        //Debug.Log(gameObject.name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            mined = true;

            if (inventory != null)
                inventory.AddMaterials(materialReward);

            Destroy(gameObject);
        }
    }
}