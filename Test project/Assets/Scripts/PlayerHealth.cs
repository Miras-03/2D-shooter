using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
