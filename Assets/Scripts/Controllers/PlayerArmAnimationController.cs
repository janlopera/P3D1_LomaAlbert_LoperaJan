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

    public void PlayAnimation(string animationName, float fadeLength = 0.1f)
    {
        armAnimation.CrossFade(animationName, fadeLength);
    }
    
}
