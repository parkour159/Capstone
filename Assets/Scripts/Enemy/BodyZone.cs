using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyZone : MonoBehaviour
{
    public EnemyHP enemyHP;
    public ParticleSystem bloodFX;

    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            enemyHP.hp -= (int)Random.Range(3f, 5.9f) * enemyHP.damage;
            ParticleSystem effect = Instantiate(bloodFX, other.transform.position, Quaternion.LookRotation(player.transform.position - other.transform.position));
            effect.Play();
        }
    }
}
