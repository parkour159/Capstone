using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimGun : MonoBehaviour
{
    public GameObject enemy, gun, enemyAvatar;
    public Transform rightHand;

    void Start()
    {
        gun.SetActive(false);
    }

    void Update()
    {
        gun.transform.position = rightHand.position;
        gun.transform.localEulerAngles = enemyAvatar.transform.localEulerAngles + new Vector3(0, -180, 0);
        if (enemy.GetComponent<SwimEnemy>().state == SwimEnemy.State.ATTACK)
        {
            gun.SetActive(true);
        }
        else
        {
            gun.SetActive(false);
        }
    }
}
