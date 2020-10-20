using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public List<KeyDoorController> ListKeys;
    public bool isOpen = false;
    public bool needKey = false;
    
    public Animation doorAnimation;
    public string openAnimationName;

    public List<LampIndicatorController> lampsIndicators;

    private void Start()
    {
        doorAnimation = this.GetComponent<Animation>();
    }

    void Update()
    {
        if (!isOpen && needKey)
        {
            bool canOpen = true;
            foreach (KeyDoorController key in ListKeys)
            {
                if (key.isUnlock == false)
                {
                    canOpen = false;
                }
            }

            if (canOpen)
            {
                OpenDoor();
            }

        }
    }

    void OpenDoorWithoutKey()
    {
        if (!isOpen && !needKey)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        doorAnimation.Play(openAnimationName);
        foreach (LampIndicatorController lamp in lampsIndicators)
        {
            lamp.setOpen();
        }
        
        isOpen = true;
    }
    
}
