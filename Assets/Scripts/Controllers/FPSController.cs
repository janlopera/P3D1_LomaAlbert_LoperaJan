using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{

    private MovementController _movementController;
    private ViewController _viewController;
    private CharacterController _characterController;


    [SerializeField]
    private GameObject _cameraObject;
    
    // Start is called before the first frame update
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        
        
        _movementController = GetComponent<MovementController>();
        _movementController.Constructor(_characterController);
        _viewController = _cameraObject.GetComponent<ViewController>();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _movementController.OnUpdate();
        _viewController.OnUpdate();
    }
}
