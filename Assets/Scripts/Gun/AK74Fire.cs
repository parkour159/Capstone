using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK74Fire : MonoBehaviour
{
    public float bulletSpeed = 60f;
    public GameObject bullet;
    public Transform barrel;
    public AudioClip ak74FireSound;
    public GameObject ak74FireEffect;

    private SetGunPos setGun;

    private void Awake()
    {
        setGun = GameObject.Find("GunManager").GetComponent<SetGunPos>();
    }
    public void GrabGun()
    {
        if (setGun != null)
            setGun.isAK74Grabbed = true;
    }

    public void ReleaseGun()
    {
        if (setGun != null)
            setGun.isAK74Grabbed = false;
    }

    public void Fire()
    {
        GameObject spawnBullet = Instantiate(bullet, barrel.position, barrel.rotation);
        spawnBullet.GetComponent<Rigidbody>().velocity = bulletSpeed * barrel.forward;
        Instantiate(ak74FireEffect, barrel.transform.position, barrel.transform.rotation);
        AudioSource.PlayClipAtPoint(ak74FireSound, transform.position);
        Destroy(spawnBullet, 2f);
    }
}
