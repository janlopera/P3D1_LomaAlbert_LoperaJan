using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class FPSController : MonoBehaviour
    {
        private CharacterController _characterController;
    
        private List<IController> _controllers;
        
        public GameObject cameraObject;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _controllers = new List<IController>
            {
                GetComponent<MovementController>(), cameraObject.GetComponent<ViewController>()
            };

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {
            _controllers.ForEach(c => c.Constructor(_characterController, this));
        }
        
        private void FixedUpdate()
        {
            _controllers.ForEach(c => c.OnUpdate());
        }
    }
}
