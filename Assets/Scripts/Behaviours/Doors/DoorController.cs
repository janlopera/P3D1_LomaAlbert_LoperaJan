using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public List<KeyDoorController> ListKeys;
    public bool isOpen = false;
    public bool needKey = false;
    
    public Animation DoorAnimation;
    public string openAnimationName;

    public List<LampIndicatorController> lampsIndicators;
    
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

    void OpenDoor()
    {
        Debug.Log("abrete sesamo");
        //DoorAnimation.CrossFade(openAnimationName);
        foreach (LampIndicatorController lamp in lampsIndicators)
        {
            lamp.setOpen();
        }
        
        isOpen = true;
    }
    
}
