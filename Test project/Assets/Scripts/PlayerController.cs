using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace Platformer
{
    public class PlayerController : MonoBehaviourPun, IPunObservable
    {
        private float movingSpeed = 7f;
        private float jumpForce = 7f;
        private float moveHorizontal;
        private float moveVertical;

        private bool facingRight = false;
        [HideInInspector] public bool deathState = false;

        private bool isGrounded;
        private bool isJumping = false;

        public Transform groundCheck;
        public Transform shootPoint;
        public Transform playerName;

        private Rigidbody2D rigidbody;
        private Animator animator;
        //private GameManager gameManager;
        private Joystick joystick;

        private PhotonView photonView;
        public TextMeshProUGUI nameText;
        private Vector3 smoothMove;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            //gameManager = FindObjectOfType<GameManager>();
            photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
            if (photonView.IsMine)
                nameText.text = PhotonNetwork.NickName;
            else
                nameText.text = photonView.Owner.NickName;
        }

        private void FixedUpdate()
        {
            if (photonView.IsMine)
                CheckGround();
        }

        private void Update()
        {
            moveHorizontal = joystick.Horizontal;
            moveVertical = joystick.Vertical;
            if (photonView.IsMine)
            {
                HandleMovement();
                HandleJump();
                HandleFlip();
            }
            else
                SmoothMove();
        }

        private void SmoothMove()
        {
            transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime);
        }

        private void HandleMovement()
        {
            if (Mathf.Abs(moveHorizontal) > 0)
            {
                Vector3 direction = transform.right * moveHorizontal;
                transform.position += direction * movingSpeed * Time.deltaTime;
                animator.SetInteger("playerState", 1);
            }
            else if (isGrounded)
                animator.SetInteger("playerState", 0);
        }

        private void HandleJump()
        {
            if (moveVertical > 0.4f && isGrounded && !isJumping)
            {
                isJumping = true;
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        private void HandleFlip()
        {
            if ((facingRight && moveHorizontal < 0) || (!facingRight && moveHorizontal > 0))
            {
                Flip();
                photonView.RPC("OnDirectionChange", RpcTarget.Others, facingRight);
            }
        }

        [PunRPC]
        private void OnDirectionChange(bool isFacingRight)
        {
            Flip();
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;

            shootPoint.Rotate(0f, 180f, 0f);
            RotatePlayerName();
        }

        private void RotatePlayerName()
        {
            float rotationY = facingRight ? 180f : 0f;
            playerName.localRotation = Quaternion.Euler(0f, rotationY, 0f);
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
            if (other.gameObject.CompareTag("Ground"))
                isJumping = false;
        }

        /*private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Coin"))
            {
                gameManager.IncrementCoins();
                Destroy(other.gameObject);
            }
        }*/

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
                stream.SendNext(transform.position);
            else
                smoothMove = (Vector3)stream.ReceiveNext();
        }
    }
}