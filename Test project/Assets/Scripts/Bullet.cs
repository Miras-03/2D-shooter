using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    private float speed = 10f;
    private int damage = 25;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;

        // Only enable collision detection and triggering for the local player's bullet
        if (!photonView.IsMine)
        {
            GetComponent<Collider2D>().enabled = false;
            rb.simulated = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (photonView.IsMine)
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
                health.TakeDamage(damage);

            // Trigger the destruction of the bullet across the network
            photonView.RPC("DestroyBullet", RpcTarget.All);
        }
    }

    [PunRPC]
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
