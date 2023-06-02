using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Platformer
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerGameObject;
        public GameObject deathPlayerPrefab;
        public Text coinText;

        private int coinsCounter = 0;
        private PlayerController player;

        private void Awake()
        {
            player = playerGameObject.GetComponent<PlayerController>();

            if (!photonView.IsMine)
                playerGameObject.SetActive(false);
        }

        private void Start()
        {
            UpdateCoinText();
        }

        private void FixedUpdate()
        {
            if (player.deathState && photonView.IsMine)
                HandlePlayerDeath();
        }

        public void HandlePlayerDeath()
        {
            //playerGameObject.SetActive(false);
            GameObject deathPlayer = PhotonNetwork.Instantiate(deathPlayerPrefab.name, playerGameObject.transform.position, playerGameObject.transform.rotation);
            deathPlayer.transform.localScale = playerGameObject.transform.localScale;
            player.deathState = false;
        }

        public void IncrementCoins()
        {
            if (photonView.IsMine)
            {
                coinsCounter++;
                UpdateCoinText();
            }
        }

        private void UpdateCoinText()
        {
            coinText.text = coinsCounter.ToString();
        }
    }
}