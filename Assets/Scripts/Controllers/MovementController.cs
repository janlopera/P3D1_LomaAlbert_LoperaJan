using Interfaces;
using UnityEngine;

namespace Controllers
{
    public class MovementController : MonoBehaviour, IController
    {
        private CharacterController _characterController;

        private Vector3 _speed;
    
    
    
        //Const
        private const float AIR_ACCELERATION = 1;
        private const float GROUND_ACCELERATION = 1;
        private const float MAX_GROUND_SPEED = 1;
        private const float DEFAULT_GROUND_FRICTION = 1;
        private const float JUMP_FORCE = 1;
        private const float GRAVITY = 1;
    

        public void Constructor(object controller)
        {
            _characterController = (CharacterController) controller;
            _speed = Vector3.zero;
        }


        public void OnUpdate()
        {

            //Get ground
            var wishVel = ComputeWishVelocity();
        
        
        
            if (!_characterController.isGrounded)
            {
                //GetInputs
                ComputeAirMovement(wishVel);
            }
            else
            {
                //GetInputs
                ComputeGroundMovement(wishVel);
            }

        }

        public void ComputeAirMovement(Vector3 wishVel)
        {
        
        
        }
    
    
    

        public void ComputeGroundMovement(Vector3 wishVel)
        {


        }

        public Vector3 ComputeWishVelocity()
        {
            Vector3 wishVel = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                wishVel.x += 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                wishVel.x += -1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                wishVel.z += 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                wishVel.z += -1;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                wishVel.y = 1;
            }
        

            return wishVel;
        }

    }
}
