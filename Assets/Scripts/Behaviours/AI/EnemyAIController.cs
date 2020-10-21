using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class EnemyAIController : MonoBehaviour
{
    // STATES
    public enum STATE
    {
        IDLE, PATROL, ALERT, CHASE, ATTACK, HIT, DIE
    }
    public STATE CurrentState;
    public HealthSystem healthSystem;
    public GameObject PlayerReference;

    public List<Transform> patrolPoints;
    public int nextPatrolPoint = 0;
    public float minDistanceToAlert = 5;

    private void Awake()
    {
        PlayerReference = GameObject.FindGameObjectWithTag("Player");
        healthSystem = this.GetComponent<HealthSystem>();
    }

    void Start()
    {
        setState(STATE.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setState(STATE newState)
    {
        if (newState == CurrentState)
        {
            return;
        }
        
        switch (newState)
        {
            case STATE.IDLE:
                Idle();
                break;
            case STATE.PATROL:
                Patrol();
                break;
            case STATE.ALERT:
                Alert();
                break;
            case STATE.CHASE:
                Chase();
                break;
            case STATE.ATTACK:
                Attack();
                break;
            case STATE.HIT:
                Hit();
                break;
            case STATE.DIE:
                Die();
                break;
        }

        CurrentState = newState;
    }
    //IDLE
    private void Idle()
    {
        
    }
    //PATROL
    private void Patrol()
    {
        //MOVE TO THE patrolPoints[nextPatrolPoint].position
        
        if (Vector3.Distance(transform.position, patrolPoints[nextPatrolPoint].position) <= 0.001f)
        {
            nextPatrolPoint++;
            if (nextPatrolPoint == patrolPoints.Count)
            {
                nextPatrolPoint = 0;
            }
        }
        
        //EXIT CONDITIONS
        
        //hears player
        if (Vector3.Distance(transform.position, PlayerReference.transform.position) <= minDistanceToAlert)
        {
            setState(STATE.ALERT);
        }

    }
    //ALERT
    private void Alert()
    {
        
    }
    
    //CHASE
    private void Chase()
    {
        
    }
    
    //ATTACK
    private void Attack()
    {
        
    }
    
    //HIT
    private void Hit()
    {
        
    }
    
    //DIE
    private void Die()
    {
        
    }
    
    
}
