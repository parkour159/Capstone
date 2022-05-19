using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory instance;
    public GameObject enemyFac;
    public float creatTime;
    public int maxCount;
    public static int count;
    GameObject enemy;

    public int COUNT
    {
        get { return count; }
        set
        {
            count = value;
            if (count < 0)
            {
                count = 0;
            }
            else if (count > maxCount)
            {
                count = maxCount;
            }
        }
    }

    private void Awake()
    {
        instance = this;
        creatTime = Random.value * 20 + 5;
        maxCount = 20;
    }

    IEnumerator Start()
    {
        while (true)
        {
            if (count < maxCount)
            {
                count++;
                enemy = Instantiate(enemyFac, this.transform.position, this.transform.rotation);
                /*Debug.Log(gameObject.name + ".,.," + count + ".,.,");*/
            }
            yield return new WaitForSeconds(creatTime);
        }
    }
}
