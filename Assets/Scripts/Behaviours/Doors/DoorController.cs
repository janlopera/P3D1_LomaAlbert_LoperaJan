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
    public string closeAnimationName;
    
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

        }else if (isOpen && needKey)
        {
            foreach (KeyDoorController key in ListKeys)
            {
                if (key.isUnlock == false)
                {
                    CloseDoor();
                }
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


    public void CloseDoor()
    {
        doorAnimation.Play(closeAnimationName);
        isOpen = false;
    }
    
}
