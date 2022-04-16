using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveEnemy : MonoBehaviour
{
    public enum State { MOVE, ATTACK, DIE }
    public State state;
    public GameObject enemyAvatar;
    Transform player;
    NavMeshAgent enemyAgent;
    public Animator ani;
    public float attackDist;
    float currentTime = 0;
    float attackDelay = 3f;

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
        enemyAvatar.transform.localPosition = new Vector3(0, -1.82f, 0);
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
        enemyAvatar.transform.localPosition = new Vector3(0, -0.82f, 0);
        enemyAvatar.transform.LookAt(player);
        ani.SetTrigger("Attack");
        if (Vector3.Distance(transform.position, player.position) <= attackDist)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                // 공격
                currentTime = 0;
            }
        }
        else
        {
            state = State.MOVE;
            currentTime = 0;
        }
    }
}