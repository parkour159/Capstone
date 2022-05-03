using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveEnemy : MonoBehaviour
{
    public enum State { MOVE, ATTACK, DIE }
    public State state;
    public GameObject enemyAvatar, boat, gun;
    Transform player;
    NavMeshAgent enemyAgent;
    public Animator ani;
    public float attackDist;
    float currentTime = 0;
    float attackDelay = 1;
    public AudioSource GunShotAudio;
    public GameObject muzzle, firePos, bullet;

    void Start()
    {
        state = State.MOVE;
        player = GameObject.Find("Final_Boat").transform;
        enemyAgent = GetComponent<NavMeshAgent>();
        attackDist = UnityEngine.Random.Range(7f, 30f);
        GetComponent<NavMeshAgent>().speed = UnityEngine.Random.value * 5 + 0.5f;
    }

    void Update()
    {
        enemyAvatar.transform.position = boat.transform.position + new Vector3(0, -0.4f, 0);
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
            enemyAgent.stoppingDistance = attackDist;
            enemyAgent.SetDestination(player.position);
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
        if (Vector3.Distance(transform.position, player.position) <= attackDist)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                // 공격
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