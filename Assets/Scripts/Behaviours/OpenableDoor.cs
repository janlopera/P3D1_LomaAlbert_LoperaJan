using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour
{

    public bool locked;
    public List<GameObject> DoorsToOpen;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    void OpenDoor()
    {
        if (!locked)
        {
            foreach (var door in DoorsToOpen)
            {
                door.GetComponent<Animation>().Play("DoorContainerOpen");
            }
        }
    }
}
