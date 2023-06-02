using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Platformer;

public class HealthBar : MonoBehaviourPun, IPunObservable
{
    public Image healthFill;
    public GameObject losePanel;

    private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        losePanel.SetActive(false);
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void HandlePlayerDeath()
    {
        gameObject.SetActive(false);
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.HandlePlayerDeath();
    }


    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();

        /*if (currentHealth <= 0f)
        {
            HandlePlayerDeath();
        }*/

        photonView.RPC("SyncHealth", RpcTarget.Others, currentHealth);
    }

    private void UpdateHealthBar()
    {
        float fillAmount = currentHealth / maxHealth;
        healthFill.fillAmount = fillAmount;
    }

    [PunRPC]
    private void SyncHealth(float newHealth)
    {
        currentHealth = newHealth;
        UpdateHealthBar();

        if (currentHealth <= 0f)
            HandlePlayerDeath();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
            if (currentHealth <= 0f)
            {
                losePanel.SetActive(true);
                HandlePlayerDeath();
            }
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            UpdateHealthBar();

            if (currentHealth <= 0f)
                HandlePlayerDeath();
        }
    }
}
