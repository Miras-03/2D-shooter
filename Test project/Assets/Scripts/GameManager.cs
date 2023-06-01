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

            // Only enable the player GameObject for the local player
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

        private void HandlePlayerDeath()
        {
            playerGameObject.SetActive(false);
            GameObject deathPlayer = PhotonNetwork.Instantiate(deathPlayerPrefab.name, playerGameObject.transform.position, playerGameObject.transform.rotation);
            deathPlayer.transform.localScale = playerGameObject.transform.localScale;
            player.deathState = false;
            Invoke(nameof(ReloadLevel), 3f);
        }

        private void ReloadLevel()
        {
            PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.Name);
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