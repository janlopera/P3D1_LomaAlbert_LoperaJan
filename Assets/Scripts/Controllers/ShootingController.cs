using System;
using System.ComponentModel;
using System.Threading.Tasks;
using FMODUnity;
using Interfaces;
using Models.Exceptions;
using Models.Weapons;
using UnityEngine;
using Utils;
using Task = UnityEditor.VersionControl.Task;

namespace Controllers
{
    public class ShootingController : MonoBehaviour, IController
    {
        private PlayerArmAnimationController _characterController;
        private FPSController _fpsController;
        private PlayerArmAnimationController _animationController;
        private HUDController _hudController;

        
        
        [SerializeField]
        private Weapon[] _weapons = new Weapon[3]; //Main, Pistol, Knife
        private int actualSlot = 0;

        private System.Threading.Tasks.Task reset, shoot, reload;
        
        public float limitStatic;
        public KeyCode reloadKey = KeyCode.R;

        private MovementController movementController;
        public async void Constructor(object state, object sender)
        {
            _fpsController = sender as FPSController;
            _animationController = _fpsController._AnimationController;
            movementController = ((FPSController) sender).gameObject.GetComponent<MovementController>();
            
            _hudController = GetComponent<HUDController>();
            InitializeWeapon(_weapons[actualSlot]);
            _hudController = GetComponent<HUDController>();
            reset = _weapons[actualSlot].Reset(this);

            reload = _weapons[actualSlot].Reload(this);

            shoot = _weapons[actualSlot].Shoot(this, null);

        }

        public void InitializeWeapon(IShootable shootable)
        {
            shootable.InjectAnimator(_animationController);
            _hudController.currentWeapon = shootable as Weapon;
        }
        

        public async void FixedUpdate()
        {
            if (Input.GetKey(reloadKey) && shoot.IsCompleted && reload.IsCompleted && reset.IsCompleted)
            {
                reload = _weapons[actualSlot].Reload(this);
                await reload;
            }
            
            if (Input.GetMouseButton(0))
            {
                try
                {
                    if (shoot.IsCompleted && reload.IsCompleted && reset.IsCompleted)
                    {
                        shoot = _weapons[actualSlot].Shoot(this, _fpsController.cameraObject);
                        await shoot;
                    }
                }
                catch (ShootFailedException e)
                {
                    switch (e.ErrorType)
                    {
                        case ShootFailedException.WeaponErrorType.NoAmmoInClip:
                            if (shoot.IsCompleted && reload.IsCompleted && reset.IsCompleted)
                            {
                                reload = _weapons[actualSlot].Reload(this);
                                await reload;
                            }
                            break;
                        case ShootFailedException.WeaponErrorType.NoAmmo:
                            //ClingClingSound
                            
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                
            }

            

            var speed = movementController.Speed.ToHorizontal().magnitude;
            var isGrounded = movementController.IsGrounded(out var normal);
            
            if (speed > limitStatic && isGrounded && (shoot.IsCompleted && reload.IsCompleted && reset.IsCompleted))
            {
                _animationController.PlayAnimation(_weapons[actualSlot].getStats().MovingAnimation);
            }
            else if((shoot.IsCompleted && reload.IsCompleted && reset.IsCompleted))
            {
                _animationController.PlayAnimation(_weapons[actualSlot].getStats().StaticAnimation);
            }
        }
    }
}