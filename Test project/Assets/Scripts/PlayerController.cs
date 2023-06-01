using UnityEngine;
using Photon.Pun;

namespace Platformer
{
    public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {
        private float movingSpeed = 5f;
        private float jumpForce = 7f;
        private float moveInput;

        private bool facingRight = false;
        [HideInInspector] public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;
        public Transform shootPoint;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        private PhotonView photonView;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = FindObjectOfType<GameManager>();
            photonView = GetComponent<PhotonView>();

            if (!photonView.IsMine)
            {
                // Disable components that should only be active for the local player
                enabled = false;
                rigidbody.simulated = false;
            }
        }

        private void FixedUpdate()
        {
            if (photonView.IsMine)
                CheckGround();
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                HandleMovement();
                HandleJump();
                HandleAnimations();
                HandleFlip();
            }
        }

        private void HandleMovement()
        {
            moveInput = Input.GetAxis("Horizontal");

            if (Mathf.Abs(moveInput) > 0)
            {
                Vector3 direction = transform.right * moveInput;
                transform.position += direction * movingSpeed * Time.deltaTime;
                animator.SetInteger("playerState", 1);
            }
            else if (isGrounded)
                animator.SetInteger("playerState", 0);
        }

        private void HandleJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        private void HandleAnimations()
        {
            if (!isGrounded)
                animator.SetInteger("playerState", 2);
        }

        private void HandleFlip()
        {
            if ((facingRight && moveInput < 0) || (!facingRight && moveInput > 0))
                Flip();
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;

            shootPoint.Rotate(0f, 180f, 0f);
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
                deathState = true;
            else
                deathState = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Coin"))
            {
                gameManager.IncrementCoins();
                Destroy(other.gameObject);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // Send deathState over the network
                stream.SendNext(deathState);
            }
            else
            {
                // Receive deathState from the network
                deathState = (bool)stream.ReceiveNext();
            }
        }
    }
}
