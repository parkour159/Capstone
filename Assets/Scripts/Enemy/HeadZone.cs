using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadZone : MonoBehaviour
{
    public GameObject head, body, enemy;
    MoveEnemy me;

    void Start()
    {
        me = enemy.GetComponent<MoveEnemy>();
    }

    void Update()
    {
        if (me.state == MoveEnemy.State.MOVE)
        {
            head.transform.localPosition = new Vector3(0, 2.5f, 0);
        }
        else if (me.state == MoveEnemy.State.ATTACK)
        {
            if (gameObject.tag == "Pistol")
            {
                head.transform.localPosition = new Vector3(0, 3, 0);
            }
            else if (gameObject.tag == "Rifle")
            {
                head.transform.localPosition = new Vector3(0, 3.1f, 0);
            }
        }
    }
}
