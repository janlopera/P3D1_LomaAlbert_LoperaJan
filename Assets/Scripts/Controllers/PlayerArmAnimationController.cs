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
    public bool isShotting = false;
    public ParticleSystem weaponFire;
    void Start()
    {
        armAnimation = this.GetComponent<Animation>();
    }
    
    void Update()
    {
        if (isShotting && !weapon.isReloading)
        {
            armAnimation.Play("1Shoot");
            weaponFire.Play();
            isShotting = false;
            return;
        }
        
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
