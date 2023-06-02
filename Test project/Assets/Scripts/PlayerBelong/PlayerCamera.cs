using UnityEngine;
using Photon.Pun;

public class PlayerCamera : MonoBehaviour
{
    public PhotonView view;
    private void Awake()
    {
        if(!view.IsMine)
            this.gameObject.SetActive(false);
    }
}
