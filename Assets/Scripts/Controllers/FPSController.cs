using System;
using System.Collections.Generic;
using Behaviours;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class FPSController : MonoBehaviour
    {
        private CharacterController _characterController;
    
        public List<IController> _controllers;
        
        public GameObject cameraObject;

        public GameObject brazo;
        public PlayerArmAnimationController _AnimationController;

        private ItemController _itemController;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _controllers = new List<IController>
            {
                GetComponent<MovementController>(), cameraObject.GetComponent<ViewController>()
            };

            _itemController = GetComponent<ItemController>();
            
            _controllers.Add(_itemController);

            var shoot = GetComponent<ShootingController>();

            _itemController._controller = shoot;
            _itemController._HealthSystem = GetComponent<HealthSystem>();
            
            _controllers.Add(shoot);

            _AnimationController = brazo.GetComponent<PlayerArmAnimationController>();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {
            _controllers.ForEach(c => c.Constructor(_characterController, this));
        }
        
        private void FixedUpdate()
        {
            //_controllers.ForEach(c => c.OnUpdate());
        }
    }
}
