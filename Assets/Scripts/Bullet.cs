using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 25;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Goblin"))
        {
            Health health = collision.collider.GetComponent<Health>();
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}