using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class ViewController : MonoBehaviour, IController
    {
        private CharacterController _characterController;

        private const float BASE_VIEW_SENSITIVITY = 5;
        public float INPUT_VIEW_SENSITIVITY = 1;
        
        
        private const float MAX_YAW = 90;
        private const float MIN_YAW = -90;

        private float pitchRotation = 0;
        
        
        
        public void Constructor(object controller, object sender)
        {
            _characterController = (CharacterController) controller;
        }
        
        public void OnUpdate()
        {
            ComputeCameraMovement();
        }
        
        private void ComputeCameraMovement()
        {
            var mouseX = Input.GetAxis("Mouse X") * BASE_VIEW_SENSITIVITY * INPUT_VIEW_SENSITIVITY;
            var mouseY = Input.GetAxis("Mouse Y") * BASE_VIEW_SENSITIVITY * INPUT_VIEW_SENSITIVITY;

            pitchRotation -= mouseY;
            pitchRotation = Mathf.Clamp(pitchRotation, MIN_YAW, MAX_YAW);
            this.transform.localRotation = Quaternion.Euler(pitchRotation, 0f, 0f);

            this.transform.parent.Rotate(Vector3.up * mouseX);


        }
    }
}
