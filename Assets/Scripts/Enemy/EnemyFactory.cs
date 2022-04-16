using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory instance;
    public GameObject enemyFac;
    public float creatTime;
    public int maxCount = 2;
    int count;
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
    }

    IEnumerator Start()
    {
        while (true)
        {
            if (count < maxCount)
            {
                count++;
                enemy = Instantiate(enemyFac, this.transform.position, this.transform.rotation);
                //enemy.transform.position = transform.position;
                //enemy.transform.rotation = transform.rotation;
            }
            yield return new WaitForSeconds(creatTime);
        }
    }
}
