using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using Utils;

[RequireComponent(typeof(HealthSystem))]
public class EnemyAIController : MonoBehaviour
{
    // STATES
    public enum STATE
    {
        IDLE, PATROL, ALERT, CHASE, ATTACK, HIT, DIE
    }
    public STATE CurrentState;
    private HealthSystem healthSystem;
    private ScoreObject scoreObject;
    private GameObject playerReference;
    public Transform raySearch;
    public float rotateVelocity = 2;

    public List<Transform> patrolPoints;
    public int nextPatrolPoint = 0;
    public float minDistanceToAlert = 10;
    public float minDistanceToAttack = 3;
    public float maxDistanceToAttack = 6;
    public float seeDistance = 10;
    private float turnAngle = 0;
    
    private void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        scoreObject = this.GetComponent<ScoreObject>();
        healthSystem = this.GetComponent<HealthSystem>();
    }

    void Start()
    {
        setState(STATE.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
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
            default:
                Idle();
                break;
        }  
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
                turnAngle = 0;
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
        setState(STATE.PATROL);
    }
    //PATROL
    private void Patrol()
    {
        //TODO:MOVE TO THE patrolPoints[nextPatrolPoint].position
        
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
        if (Vector3.Distance(transform.position, playerReference.transform.position) <= minDistanceToAlert)
        {
            setState(STATE.ALERT);
        }

    }
    //ALERT
    private void Alert()
    {
        //ROTATE
        transform.Rotate(0, rotateVelocity * Time.deltaTime , 0 , Space.Self);
        turnAngle += rotateVelocity;
        
        //EXIT CONDITIONS

        if (SeesPlayer())
        {
            if (Vector3.Distance(transform.position, playerReference.transform.position).Between(minDistanceToAttack, maxDistanceToAttack, true)) //sees player and go to attack if is in distance
            {
                setState(STATE.ATTACK);
            }
            else //sees player and go to chase if is NOT in distance
            {
                setState(STATE.CHASE);   
            }
        }
        
        //ends the turn and !seesPlayer --> return to patrol
        if (turnAngle >= 360.0f)
        {
            setState(STATE.PATROL);
        }
        

    }

    private bool SeesPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(raySearch.position, raySearch.forward,out hit,seeDistance))
        {
            return hit.transform.gameObject.CompareTag("Player");
        }

        return false;
    }
    
    //CHASE
    private void Chase()
    {
        //Go to the player while distance > maxAttackDistance
        //TODO:MOVE
        
        //EXIT CONDITIONS

        var distanceToPlayer = Vector3.Distance(transform.position, playerReference.transform.position);
        
        //distance <= maxAttackDistance
        if (SeesPlayer() && distanceToPlayer.Between(minDistanceToAttack, maxDistanceToAttack, true))
        {
            setState(STATE.ATTACK);
        }else if (!SeesPlayer()) //not seeing player
        {
            setState(STATE.ALERT);
        }
        


    }
    
    //ATTACK
    private void Attack()
    {
        //TODO:DISPARA
        
        //EXIT CONDITIONS 
        if (!Vector3.Distance(transform.position, playerReference.transform.position)
            .Between(minDistanceToAttack, maxDistanceToAttack, true))
        {
            setState(STATE.ALERT);
        }
        
    }
    
    //HIT
    private void Hit()
    {
        
    }

    public void onHit()
    {
        if (healthSystem.Health > 0)
        {
            setState(STATE.HIT);
        }
        else
        {
            setState(STATE.DIE);
        }
        
    }
    
    //DIE
    private void Die()
    {
        //TODO:EXPLODE AND DIE
    }
    
    
}
