using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    { 
        OnJoinedLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene(1);
    }
}