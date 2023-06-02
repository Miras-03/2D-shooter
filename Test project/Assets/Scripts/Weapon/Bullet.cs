using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    private float speed = 10f;
    private int damage = 25;
    private Rigidbody2D rb;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("First");
            HealthBar health = other.GetComponent<HealthBar>();
            if (health != null)
                health.TakeDamage(damage);
        }
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }
}
