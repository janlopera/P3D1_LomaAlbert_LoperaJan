using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtPlayer : MonoBehaviour
{
    private Transform playerRefenrence;
    public bool isLooking = true;
    public bool isWorldUp = true;
    private void Awake()
    {
        playerRefenrence = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLooking)
        {
            transform.LookAt(playerRefenrence);

        }
    }
}
