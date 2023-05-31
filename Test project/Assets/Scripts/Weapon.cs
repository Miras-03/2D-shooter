using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject bullet;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
            Shoot();
    }

    private void Shoot()
    {
        Instantiate(bullet, shootPoint.position, shootPoint.rotation);
    }
}
