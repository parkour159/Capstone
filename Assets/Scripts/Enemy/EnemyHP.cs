using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int hp;
    public int damage;

    private void Start()
    {
        hp = 100;
    }

    void Update()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
            EnemyFactory.count--;
        }
    }
}
