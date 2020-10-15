using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class PlayerArmAnimationController : MonoBehaviour
{
    public Animation armAnimation;
    public MovementController movementController;
    public float limitStatic;
    void Start()
    {
        armAnimation = this.GetComponent<Animation>();
    }
    
    void Update()
    {
        float speed = movementController.Speed.magnitude;

        if (speed < limitStatic)
        {
            armAnimation.CrossFade("StaticWeaponAK47", 0.2f);
        }
        else
        {
            armAnimation.CrossFade("MoveWeaponAK47", 0.2f);
        }
    }
}
