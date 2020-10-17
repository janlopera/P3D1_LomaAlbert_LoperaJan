using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour //PROVISIONAL
{
    public PlayerArmAnimationController PlayerArmAnimationController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !PlayerArmAnimationController.weapon.isReloading)
        {
            PlayerArmAnimationController.isShotting = true;
        }
    }
}
