using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 10f;
    private int damage = 25;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
            health.TakeDamage(damage);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy (gameObject);
    }
}
