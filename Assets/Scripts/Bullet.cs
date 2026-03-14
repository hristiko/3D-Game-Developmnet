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
        
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
            return;
        }

        Health health = collision.collider.GetComponentInParent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

}
