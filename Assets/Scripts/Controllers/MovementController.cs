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

        public GameObject GroundDetectors;
        
        [SerializeField]
        private List<Transform> _groundRayOrigins;

        [SerializeField]
        private CapsuleCollider _capsuleCollider;
        
        
        private CharacterController _characterController;

        private Vector3 _speed;

        private FPSController _fpsController;
        
        
        private const float AIR_ACCELERATION = 1f;
        private const float AIR_DECELERATION = 2.5f;
        private const float GROUND_ACCELERATION = 20f;
        private const float MAX_GROUND_SPEED = 5f;
        private const float DEFAULT_GROUND_FRICTION = 8f;
        private const float JUMP_FORCE = 8f;
        private const float GRAVITY = 24f;
        private const float FRICTION_MIN_SPEED = 0.5f;
        
        private readonly Collider[] _overlappingColliders = new Collider[5];
        
        private bool _willJump;
        private bool _isGroundedInPrevFrame;
        private bool _isSpacePressedPrev;

        public Vector3 Speed => _speed;


        public void Constructor(object controller, object sender)
        {
            _characterController = (CharacterController) controller;
            _speed = Vector3.zero;
            _fpsController = sender as FPSController;
            _isGroundedInPrevFrame = false;
            _willJump = false;
            _isSpacePressedPrev = false;
           _groundRayOrigins = new List<Transform>(GroundDetectors.GetComponentsInChildren<Transform>());
           _groundRayOrigins.Remove(GroundDetectors.transform);
        }
        

        public void FixedUpdate()
        {
            var wishVel = ComputeWishVelocity();
            ComputeJump();
            wishVel = _fpsController.cameraObject.transform.parent.TransformDirToHorizontal(wishVel);

            var isGrounded = IsGrounded(out var groundNormal);

            if (isGrounded || _isGroundedInPrevFrame)
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
            
            if (displacement.magnitude > 0.1f)
            {
                ClampDisplacement(ref _speed, ref displacement, transform.position);
            }


            var position = transform.position;
            position += displacement;

            var collisionDisplacement = ResolveCollisions(ref _speed);

            transform.position += collisionDisplacement;
            _isGroundedInPrevFrame = isGrounded;
            
            
            position += _speed * Time.deltaTime;
            transform.position = position;

            _willJump = false;

        }

        private void ClampDisplacement(ref Vector3 playerVelocity, ref Vector3 displacement, Vector3 playerPosition)
        {
            if (Physics.Raycast(playerPosition, playerVelocity.normalized, out var hit, displacement.magnitude, ~_excludedLayers))
            {
                displacement = hit.point - playerPosition;
            }
        }
        
        private Vector3 ResolveCollisions(ref Vector3 playerVelocity)
        {
            
            // Get nearby colliders
            Physics.OverlapSphereNonAlloc(transform.position, _capsuleCollider.height + 0.5f,
                _overlappingColliders, ~_excludedLayers);
            
            var totalDisplacement = Vector3.zero;
            var checkedColliderIndices = new HashSet<int>();

            Vector3 pvel = Vector3.zero;
           
            // If the player is intersecting with that environment collider, separate them
            for (var i = 0; i < _overlappingColliders.Length; i++)
            {
                // Two player colliders shouldn't resolve collision with the same environment collider
                if (checkedColliderIndices.Contains(i))
                {
                    continue;
                }

                var envColl = _overlappingColliders[i];

                // Skip empty slots
                if (envColl == null)
                {
                    continue;
                }

                if (!Physics.ComputePenetration(
                    _capsuleCollider, _capsuleCollider.transform.position, _capsuleCollider.transform.rotation,
                    envColl, envColl.transform.position, envColl.transform.rotation,
                    out var collisionNormal, out var collisionDistance)) continue;
                // Ignore very small penetrations
                // Required for standing still on slopes
                // ... still far from perfect though
                if (collisionDistance < 0.015)
                {
                    continue;
                }

                checkedColliderIndices.Add(i);

                // Get outta that collider!
                totalDisplacement += collisionNormal * collisionDistance;
                //Debug.Log($"T: {collisionNormal * collisionDistance}, N: {collisionNormal}, D: {collisionDistance}");
                // Crop down the velocity component which is in the direction of penetration
                pvel -= Vector3.Project(playerVelocity, collisionNormal);
                //Debug.Log($"Bug: {Vector3.Reflect(playerVelocity, collisionNormal )}, pVel: {playerVelocity}, norm: {collisionNormal}");
            }

            playerVelocity += pvel;
            
            // It's better to be in a clean state in the next resolve call
            for (var i = 0; i < _overlappingColliders.Length; i++)
            {
                _overlappingColliders[i] = null;
            }
            return totalDisplacement;

        }

        private float GetProperAirAcceleration(Vector3 speed, Vector3 wishVel)
        {
            return Vector3.Dot(speed, wishVel) > 0 ? AIR_ACCELERATION : AIR_DECELERATION;
        }

        private void ComputeJump()
        {
            if (Input.GetKey(KeyCode.Space) && !_willJump)
            {
                _willJump = true;
                _isSpacePressedPrev = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _isSpacePressedPrev = false;
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

        public bool IsGrounded(out Vector3 normal)
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
