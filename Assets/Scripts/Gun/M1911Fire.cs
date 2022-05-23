using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class M1911Fire : MonoBehaviour
{
    public float bulletSpeed = 40f;
    public GameObject bullet;
    public Transform barrel;
    public AudioClip m1911FireSound;
    public GameObject m1911FireEffect;

    private SetGunPos setGun;
    private Swimmer swimmer;

    private void Awake()
    {
        if (gameObject.scene.name == "FPSUnderWater")
        {
            swimmer = GameObject.Find("Player").GetComponent<Swimmer>();
        }
        else if (gameObject.scene.name == "FPSontheboatPhyscisHand")
        {
            setGun = GameObject.Find("GunManager").GetComponent<SetGunPos>();
        }
    }

    public void GrabGun()
    {
        if (gameObject.scene.name == "FPSUnderWater")
        {
            if (swimmer != null)
            {
                swimmer.isGunGrabbed = true;
            }
        }
        else if (gameObject.scene.name == "FPSontheboatPhyscisHand")
        {
            if (setGun != null)
            {
                setGun.isM1911Grabbed = true;
            }
        }

    }

    public void ReleaseGun()
    {
        if (gameObject.scene.name == "FPSUnderWater")
        {
            if (swimmer != null)
            {
                swimmer.isGunGrabbed = false;
            }
        }
        else if (gameObject.scene.name == "FPSontheboatPhyscisHand")
        {
            if (setGun != null)
            {
                setGun.isM1911Grabbed = false;
            }
        }
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
