using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{

    public Vector3 movementVector;
    public float velocity;
    public bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = transform.position + movementVector * velocity * Time.deltaTime; 
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Detection")))
        {
            velocity = -velocity;
        }
    }
}
