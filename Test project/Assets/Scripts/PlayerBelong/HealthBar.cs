using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HealthBar : MonoBehaviourPun, IPunObservable
{
    public Image healthFill;

    private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0f)
        {
            // Handle player death
        }

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
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(currentHealth);
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            UpdateHealthBar();
        }
    }
}
