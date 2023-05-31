using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public GameObject playerGameObject;
        public GameObject deathPlayerPrefab;
        public Text coinText;

        private int coinsCounter = 0;
        private PlayerController player;

        private void Awake()
        {
            player = playerGameObject.GetComponent<PlayerController>();
        }

        private void Start()
        {
            UpdateCoinText();
        }

        private void FixedUpdate()
        {
            if (player.deathState)
                HandlePlayerDeath();
        }

        private void HandlePlayerDeath()
        {
            playerGameObject.SetActive(false);
            GameObject deathPlayer = Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
            deathPlayer.transform.localScale = playerGameObject.transform.localScale;
            player.deathState = false;
            Invoke(nameof(ReloadLevel), 3f);
        }

        private void ReloadLevel()
        {
            int activeSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(activeSceneIndex);
        }

        public void IncrementCoins()
        {
            coinsCounter++;
            UpdateCoinText();
        }

        private void UpdateCoinText()
        {
            coinText.text = coinsCounter.ToString();
        }
    }
}