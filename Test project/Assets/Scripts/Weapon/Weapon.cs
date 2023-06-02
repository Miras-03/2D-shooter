using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviourPun
{
    public Transform shootPoint;
    public GameObject bulletPrefab;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (photonView.IsMine)
                Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, shootPoint.position, shootPoint.rotation);
    }
}
