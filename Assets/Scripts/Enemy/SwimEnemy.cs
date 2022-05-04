using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwimEnemy : MonoBehaviour
{
    public enum State { MOVE, ATTACK, DIE }
    public State state;
    public GameObject enemyAvatar, gun;
    Transform player;
    public Animator ani;
    public float attackDist;
    float currentTime = 0;
    float attackDelay = 1;
    public AudioSource GunShotAudio;
    public GameObject muzzle, firePos, bullet;
    RaycastHit hit;

    void Start()
    {
        state = State.MOVE;
        player = GameObject.Find("Player").transform;
        attackDist = UnityEngine.Random.Range(7f, 30f);
    }

    void Update()
    {
        if (state == State.MOVE)
        {
            Move();
        }
        else if (state == State.ATTACK)
        {
            Attack();
        }
    }

    void Move()
    {
        enemyAvatar.transform.localEulerAngles = new Vector3(0, 0, 0);
        ani.SetTrigger("Move");
        if (Vector3.Distance(transform.position, player.position) > attackDist)
        {
            Vector3 dir = player.position - transform.position;
            dir = dir.normalized;
            transform.position += dir * (Random.value * 5 + 0.5f) * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(dir);
        }
        else
        {
            state = State.ATTACK;
            currentTime = attackDelay;
        }
    }

    void Attack()
    {
        enemyAvatar.transform.LookAt(player);
        ani.SetTrigger("Attack");
/*        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.tag == "Map")
            {
                attackDist /= 2;
            }
        }*/
        if (Vector3.Distance(transform.position, player.position) <= attackDist)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                // АјАн
                GunFire();
                currentTime = 0;
            }
        }
        else
        {
            state = State.MOVE;
            currentTime = 0;
        }
    }

    void GunFire()
    {
        GunShotAudio.Play();
        if (gameObject.tag == "Pistol")
        {
            firePos.transform.localPosition = new Vector3(0, 0, -0.15f);
            attackDelay = 1.167f;
        }
        else if (gameObject.tag == "Rifle")
        {
            firePos.transform.localPosition = new Vector3(0, 0, -0.45f);
            attackDelay = 1.1f;
        }
        firePos.transform.localEulerAngles = new Vector3(0, 90, 0);
        Instantiate(muzzle, firePos.transform);
        Instantiate(bullet, firePos.transform.position, Quaternion.identity);
    }
}
