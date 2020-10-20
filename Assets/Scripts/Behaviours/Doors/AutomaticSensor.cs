using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticSensor : MonoBehaviour
{
    public LampIndicatorController lamp;
    public KeyDoorController keyDoorSensor;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            keyDoorSensor.isUnlock = true;
        }

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            keyDoorSensor.isUnlock = false;
        }
    }
}
