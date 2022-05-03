using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1911Fire : MonoBehaviour
{
    public float bulletSpeed = 40f;
    public GameObject bullet;
    public Transform barrel;
    public SetGunPos setGun;

    public void GrabGun()
    {
        setGun.isM1911Grabbed = true;
    }

    public void ReleaseGun()
    {
        setGun.isM1911Grabbed = false;
    }

    public void Fire()
    {
        GameObject spawnBullet = Instantiate(bullet, barrel.position, barrel.rotation);
        spawnBullet.GetComponent<Rigidbody>().velocity = bulletSpeed * barrel.forward;
        Destroy(spawnBullet, 2f);
    }
}
