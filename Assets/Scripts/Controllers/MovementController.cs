using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class MovementController : MonoBehaviour, IController
    {
        [SerializeField]
        private LayerMask _excludedLayers;
        
        [SerializeField]
        private List<Transform> _groundRayOrigins;

        [SerializeField]
        private CapsuleCollider _capsuleCollider;
        
        
        private CharacterController _characterController;

        private Vector3 _speed;

        private FPSController _fpsController;
        
        
    
        //Const
        private const float AIR_ACCELERATION = 1f;
        private const float AIR_DECELERATION = 1.5f;
        private const float GROUND_ACCELERATION = 500f;
        private const float MAX_GROUND_SPEED = 8f;
        private const float DEFAULT_GROUND_FRICTION = 15f;
        private const float JUMP_FORCE = 8f;
        private const float GRAVITY = 24f;
        private const float FRICTION_MIN_SPEED = 0.5f;
        
        private readonly Collider[] _overlappingColliders = new Collider[5];


        private bool _willJump;
        private bool _isGroundedInPrevFrame;
    

        public void Constructor(object controller, object sender)
        {
            _characterController = (CharacterController) controller;
            _speed = Vector3.zero;
            _fpsController = sender as FPSController;
            _isGroundedInPrevFrame = false;
            _willJump = false;
        }

        public void OnUpdate1()
        {
            
        }

        public void OnUpdate()
        {
            var wishVel = ComputeWishVelocity();
            ComputeJump();
            wishVel = _fpsController._cameraObject.transform.parent.TransformDirToHorizontal(wishVel);

            var isGrounded = IsGrounded(out var groundNormal);

            if (isGrounded)
            {
                if (!_willJump)
                {
                    ComputeFriction(ref _speed, Time.deltaTime);
                }
                ComputeMovement(ref _speed, wishVel, GROUND_ACCELERATION, Time.deltaTime);
                _speed = Vector3.ProjectOnPlane(_speed, groundNormal);
                ComputeJump(_willJump, ref _speed);
            }
            else //We're flying boys!
            {
                var accel = GetProperAirAcceleration(_speed, wishVel);
                
                ComputeMovement(ref _speed, wishVel, accel, Time.deltaTime);
                
                
                _speed += Vector3.down * (GRAVITY * Time.deltaTime);
            }
            
            
            var displacement = _speed * Time.deltaTime;
        
            // If we're moving too fast, make sure we don't hollow through any collider
            if (displacement.magnitude > _capsuleCollider.radius)
            {
                ClampDisplacement(ref _speed, ref displacement, transform.position);
            }
            
            
            transform.position += displacement;

            var collisionDisplacement = ResolveCollisions(ref _speed);

            //transform.position += collisionDisplacement;
            _isGroundedInPrevFrame = isGrounded;
            
            
            transform.position += _speed * Time.deltaTime;

        }

        private void ClampDisplacement(ref Vector3 playerVelocity, ref Vector3 displacement, Vector3 playerPosition)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerPosition, playerVelocity.normalized, out hit, displacement.magnitude, ~_excludedLayers))
            {
                displacement = hit.point - playerPosition;
            }
        }
        
        private Vector3 ResolveCollisions(ref Vector3 playerVelocity)
        {
            return playerVelocity;

        }

        private float GetProperAirAcceleration(Vector3 speed, Vector3 wishVel)
        {
            return Vector3.Dot(speed, wishVel) > 0 ? AIR_ACCELERATION : AIR_DECELERATION;
        }

        private void ComputeJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_willJump)
            {
                _willJump = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                _willJump = false;
            }
        }

        private void ComputeJump(bool willJump, ref Vector3 spd)
        {
            if (willJump)
            {
                spd += -Vector3.down * JUMP_FORCE;
            }
        }


        private void ComputeMovement(ref Vector3 playerVelocity, Vector3 wishVel, float accelCft, float dt)
        {
            var currentProjSpeed = Vector3.Dot(playerVelocity, wishVel);
            
            var addSpeed = MAX_GROUND_SPEED - currentProjSpeed;
            if (addSpeed <= 0) return;
            
            var accelAmount = accelCft * MAX_GROUND_SPEED * dt;
            
            if (accelAmount > addSpeed) accelAmount = addSpeed;
                
            playerVelocity += wishVel * accelAmount;
            
            
        }
        
        private void ComputeFriction(ref Vector3 playerVel, float dt)
        {
            var speed = playerVel.magnitude;
            
            if (speed <= 0.001) return;
            
            var downLimit = Mathf.Max(speed, FRICTION_MIN_SPEED);
            var dropAmount = speed - (downLimit * DEFAULT_GROUND_FRICTION * dt);
            if (dropAmount < 0) dropAmount = 0;
            
            playerVel *= dropAmount / speed;
            
        }

        private Vector3 ComputeWishVelocity()
        {
            var wishVel = new Vector3 {x = Input.GetAxisRaw("Horizontal"), z = Input.GetAxisRaw("Vertical")};
            return wishVel;
        }

        private bool IsGrounded(out Vector3 normal)
        {
            normal = -Vector3.down;

            var isGrounded = false;
            foreach (var r in _groundRayOrigins)
            {
                if (!Physics.Raycast(r.position, Vector3.down, out var hit, 0.5f, ~_excludedLayers)) continue;
                normal = hit.normal;
                isGrounded = true;
            }

            return isGrounded;
        }

    }
}
