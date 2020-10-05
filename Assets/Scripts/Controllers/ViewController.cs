using Interfaces;
using UnityEngine;

namespace Controllers
{
    public class ViewController : MonoBehaviour, IController
    {
        private CharacterController _characterController;
    
    
    
        public void Constructor(object controller)
        {
            _characterController = (CharacterController) controller;
        }
        void Start()
        {
        
        }

        // Update is called once per frame
        public void OnUpdate()
        {
        
        }
    }
}
