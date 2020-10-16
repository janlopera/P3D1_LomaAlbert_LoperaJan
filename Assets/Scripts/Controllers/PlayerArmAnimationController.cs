using System.Collections;
using System.Collections.Generic;
using Controllers;
using Models.Weapons;
using UnityEngine;
using Utils;

public class PlayerArmAnimationController : MonoBehaviour
{
    public Animation armAnimation;
    public MovementController movementController;
    public float limitStatic;
    public Weapon weapon;
    void Start()
    {
        armAnimation = this.GetComponent<Animation>();
    }
    
    void Update()
    {
        float speed = movementController.Speed.ToHorizontal().magnitude;
        bool isGrounded = movementController.IsGrounded(out var normal);
        if (weapon.isReloading)
        {
            armAnimation.CrossFade("RechargeWeaponAK47", 0.1f);
            return;
        }
        if (speed > limitStatic && isGrounded)
        {
            armAnimation.CrossFade("MoveWeaponAK47", 0.1f);
        }
        else
        {
            armAnimation.CrossFade("StaticWeaponAK47", 0.1f);
        }
    }
}
