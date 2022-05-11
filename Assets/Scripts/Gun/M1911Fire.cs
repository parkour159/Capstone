using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class M1911Fire : MonoBehaviour
{
    public XRController hand;
    public float bulletSpeed = 40f;
    public GameObject bullet;
    public Transform barrel;
    public SetGunPos setGun;
    public AudioClip m1911FireSound;
    public GameObject m1911FireEffect;

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
        Instantiate(m1911FireEffect, barrel.transform.position, barrel.transform.rotation);
        AudioSource.PlayClipAtPoint(m1911FireSound, transform.position);
        Destroy(spawnBullet, 2f);
    }
}
