using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Behaviours;
using FMODUnity;
using Models.Weapons;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;
using Utils;
using Random = System.Random;

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
    public List<Transform> seeRays;
    public float rotateVelocity = 2;
    public Transform hitEffectLocation;
    public ParticleSystem hitEffect;

    public ShootController wp;

    public NavMeshAgent agent;

    public List<Transform> patrolPoints;
    public int nextPatrolPoint = 0;
    public float minDistanceToAlert = 10;
    public float minDistanceToAttack = 3;
    public float maxDistanceToAttack = 6;
    public float seeDistance = 10;
    public float turnAngle = 0;
    public int turnDirecction = 1;

    private Task _soundTask;

    private StudioEventEmitter _eventEmitter;
    
    private void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        scoreObject = this.GetComponent<ScoreObject>();
        healthSystem = this.GetComponent<HealthSystem>();
    }

    private async Task playSoundwDelay(string sound)
    {
        _eventEmitter.Event = sound;
        _eventEmitter.Play();
        
        await Task.Delay(6000);
    }
    

    async void Start()
    {
        setState(STATE.IDLE);
        _eventEmitter = GetComponent<StudioEventEmitter>();
        _soundTask = playSoundwDelay("event:/Enemy/Alert");
        await _soundTask;
    }

    // Update is called once per frame
    async void Update()
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

    async void setState(STATE newState)
    {
        if (newState == CurrentState)
        {
            return;
        }

        agent.SetDestination(transform.position);
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
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        CurrentState = newState;
    }
    //IDLE
    private async void Idle()
    {
        setState(STATE.PATROL);
    }
    //PATROL
    private async void Patrol()
    {
        
        if (_soundTask.IsCompleted)
        {
            _soundTask = playSoundwDelay("event:/Enemy/Patrol");
            await _soundTask;
        }
        
        agent.angularSpeed = 120;
        agent.SetDestination(patrolPoints[nextPatrolPoint].position);
        if (Vector3.Distance(transform.position, patrolPoints[nextPatrolPoint].position) <= 0.3f)
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
            if (_soundTask.IsCompleted)
            {
                _soundTask = playSoundwDelay("event:/Enemy/Alert");
                await _soundTask;
            }
            setState(STATE.ALERT);
        }

    }
    //ALERT
    private async void Alert()
    {
     
        if (_soundTask.IsCompleted)
        {
            _soundTask = playSoundwDelay("event:/Enemy/Alert");
            await _soundTask;
        }
        
        //EXIT CONDITIONS

        if (SeesPlayer())
        {
            transform.Rotate(0, 0 , 0 , Space.Self);
            if (Vector3.Distance(transform.position, playerReference.transform.position).Between(minDistanceToAttack, maxDistanceToAttack, true)) //sees player and go to attack if is in distance
            {
                setState(STATE.ATTACK);
            }
            else //sees player and go to chase if is NOT in distance
            {
                setState(STATE.CHASE);   
            }
        }
        
        //ROTATE
        transform.Rotate(0, rotateVelocity * turnDirecction , 0 , Space.Self);
        turnAngle += Mathf.Abs(rotateVelocity);
        
        //ends the turn and !seesPlayer --> return to patrol
        if (turnAngle >= 361.0f)
        {
            setState(STATE.PATROL);
        }
        

    }

    private bool SeesPlayer()
    {
        //Debug.DrawRay(raySearch.position, raySearch.forward, Color.red);
        bool isSeen = false;
        foreach (Transform seeRay in seeRays)
        {
            RaycastHit hit;
            if(Physics.Raycast(seeRay.position, seeRay.forward,out hit,seeDistance) && hit.transform.gameObject.CompareTag("Player"))
            {
                isSeen = true;
            }
        }

        return isSeen;
    }

    //CHASE
    private async void Chase()
    {
        
        if (_soundTask.IsCompleted)
        {
            _soundTask = playSoundwDelay("event:/Enemy/NoAmmo");
            await _soundTask;
        }
        
        agent.angularSpeed = 0;
        var playerPosition = playerReference.transform.position;
        
        var distanceToPlayer = Vector3.Distance(transform.position.ToHorizontal(), playerPosition.ToHorizontal());
        
        if (!distanceToPlayer.Between(minDistanceToAttack, maxDistanceToAttack, true))
        {
            
            var position = this.transform.position;
            var dist = playerPosition - position;

            var trueDist = dist / 5f;
            
            if (distanceToPlayer > minDistanceToAttack)
            {
                agent.SetDestination(trueDist + position);
            }
            else
            {
                transform.LookAt(playerReference.transform);
                agent.SetDestination(trueDist.Inverse() + position);
            }
        }
        
        //EXIT CONDITIONS

        
        
        //distance <= maxAttackDistance
        if (distanceToPlayer.Between(minDistanceToAttack, maxDistanceToAttack, true))
        {
            
            setState(STATE.ATTACK);
        }else if (!SeesPlayer()) //not seeing player
        {
            setState(STATE.ALERT);
        }
        


    }
    
    //ATTACK
    private async void Attack()
    {

        await wp.Shoot();
        
        if (_soundTask.IsCompleted)
        {
            _soundTask = playSoundwDelay("event:/Enemy/Hit");
            await _soundTask;
        }
        if (!SeesPlayer())
        {
            setState(STATE.ALERT);
        }
        
        //EXIT CONDITIONS 
        if (!Vector3.Distance(transform.position, playerReference.transform.position)
            .Between(minDistanceToAttack, maxDistanceToAttack, true))
        {
            setState(STATE.ALERT);
        }
        
    }
    
    //HIT
    private async void Hit()
    {
        PlayHitAnimation();
        
        if (_soundTask.IsCompleted)
        {
            _soundTask = playSoundwDelay("event:/Enemy/Alert");
            await _soundTask;
        }
        
        setState(STATE.ALERT);
    }

    private void PlayHitAnimation()
    {
        //HIT ANIMATION
        var effect = Instantiate(hitEffect, hitEffectLocation);
        Destroy(effect, 2.0f);
    }

    public void onHit()
    {
        if (healthSystem.Health > 0)
        {
            if (CurrentState != STATE.ATTACK)
            {
                setState(STATE.HIT);
            }
            else
            {
                PlayHitAnimation();
            }

        }
        else
        {
            setState(STATE.DIE);
        }
        
    }
    
    //DIE
    private void Die()
    {
        
    }
    
    
}
