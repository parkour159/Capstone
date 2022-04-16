using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject enemy, gun;
    public Transform rightHand;

    void Start()
    {
        gun.SetActive(false);
        gun.transform.eulerAngles = new Vector3(0, -180, 0);
    }

    void Update()
    {
        gun.transform.position = rightHand.position;

        if (enemy.GetComponent<MoveEnemy>().state == MoveEnemy.State.ATTACK)
        {
            gun.SetActive(true);
        }
        else
        {
            gun.SetActive(false);
        }
    }
}
