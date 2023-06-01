using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] points;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            int rand = Random.Range(0, points.Length);
            Vector3 randomPosition = points[rand].position;
            PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        }
        else
            Debug.LogError("Photon is not connected. Make sure you have the PhotonView component on the player prefab.");
    }
}
