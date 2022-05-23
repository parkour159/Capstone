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
    public GameObject[] secondGrabPoints;

    private SetGunPos setGun;
    private Swimmer swimmer;

    private void Awake()
    {
        if(gameObject.scene.name == "FPSUnderWater")
        {
            swimmer = GameObject.Find("Player").GetComponent<Swimmer>();
        }
        else if(gameObject.scene.name == "FPSontheboatPhyscisHand")
        {
            setGun = GameObject.Find("GunManager").GetComponent<SetGunPos>();
        }
        for(int i = 0; i<secondGrabPoints.Length; i++)
        {
            secondGrabPoints[i].SetActive(false);
        }
    }
    public void GrabGun()
    {
        if (gameObject.scene.name == "FPSUnderWater")
        {
            if(swimmer != null)
            {
                swimmer.isGunGrabbed = true;
            }
        }
        else if (gameObject.scene.name == "FPSontheboatPhyscisHand")
        {
            if (setGun != null)
            {
                setGun.isAK74Grabbed = true;
            }    
        }
        for (int i = 0; i < secondGrabPoints.Length; i++)
        {
            if(secondGrabPoints[i].activeSelf == false)
            {
                secondGrabPoints[i].SetActive(true);
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
                setGun.isAK74Grabbed = false;
            }
        }
        for (int i = 0; i < secondGrabPoints.Length; i++)
        {
            if (secondGrabPoints[i].activeSelf == true)
            {
                secondGrabPoints[i].SetActive(false);
            }
        }
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
