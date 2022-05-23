using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP_Bar : MonoBehaviour
{
    public Slider hp_Bar;
    public EnemyHP enemyHP;

    private void Update()
    {
        hp_Bar.value = enemyHP.hp;
    }
}
