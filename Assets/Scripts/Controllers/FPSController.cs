using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Controllers
{
    public class FPSController : MonoBehaviour
    {
        private CharacterController _characterController;
    
        private List<IController> _controllers;
    


        [SerializeField]
        private GameObject _cameraObject;
    
        // Start is called before the first frame update
        private void Awake()
        {
            _controllers = new List<IController>
            {
                GetComponent<MovementController>(), _cameraObject.GetComponent<ViewController>()
            };


        }

        private void Start()
        {
            _controllers.ForEach(c => c.Constructor(_characterController));
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            _controllers.ForEach(c => c.OnUpdate());
        }
    }
}
