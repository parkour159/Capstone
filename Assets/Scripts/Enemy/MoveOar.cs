using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOar : MonoBehaviour
{
    public Transform lefthand, righthand;

    public GameObject player, enemy;
    MoveEnemy me;

    Vector3 dir;
    Quaternion oarRotation;

    float continuousTime, firstTime, secondTime, totalTime;

    void Start()
    {
        player = GameObject.Find("Final_Boat");
        me = enemy.GetComponent<MoveEnemy>();

        continuousTime = 0;
        firstTime = 0.25f;
        secondTime = 2.05f;
        totalTime = 3.633f;
    }

    void Update()
    {
        continuousTime += Time.deltaTime;

        if (Vector3.Distance(player.transform.position, enemy.transform.position) <= me.attackDist)
        {
            transform.localPosition = new Vector3(-0.009f, 0.0017f, 0.004f);
            transform.localEulerAngles = new Vector3(0, -90, 0);
        }
        else
        {
            if (continuousTime < firstTime)
            {
                transform.position = righthand.position;

                dir = (lefthand.position - righthand.position).normalized;
                oarRotation = Quaternion.LookRotation(dir);
                transform.rotation = oarRotation;
            }
            else if (continuousTime < secondTime)
            {
                transform.position = lefthand.position;

                dir = (righthand.position - lefthand.position).normalized;
                oarRotation = Quaternion.LookRotation(dir);
                transform.rotation = oarRotation;
            }
            else if (continuousTime < totalTime)
            {
                transform.position = righthand.position;

                dir = (lefthand.position - righthand.position).normalized;
                oarRotation = Quaternion.LookRotation(dir);
                transform.rotation = oarRotation;
            }
            else if (continuousTime >= totalTime)
            {
                continuousTime = 0;
            }
        }
    }
}
