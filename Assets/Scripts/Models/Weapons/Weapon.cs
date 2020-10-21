using System;
using System.Net.Configuration;
using System.Threading.Tasks;
using Behaviours;
using FMODUnity;
using Interfaces;
using Manager;
using Models.Exceptions;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Models.Weapons
{
    
    public class Weapon : MonoBehaviour, IShootable
    {
        public WeaponStats WeaponStats;

        public int BulletsPerClip;
        public int BulletsLeft;

        public bool canShoot = false;
        public bool isReloading = false;
        
        private PlayerArmAnimationController _animationController;
        
        public ParticleSystem weaponFire;
        
        public StudioEventEmitter _sound;

        public LayerMask LayerMask;

        private bool CanShoot()
        {
            return canShoot;
        }

        public bool isAuto()
        {
            return WeaponStats.IsFullAuto;
        }

        public void InjectAnimator(PlayerArmAnimationController armAnimationController)
        {
            _animationController = armAnimationController;
            _sound = GetComponent<StudioEventEmitter>();
        }

        public WeaponStats getStats()
        {
            return WeaponStats;
        }

        public void Start()
        {
            BulletsLeft = WeaponStats.ReserveAmo;
            BulletsPerClip = WeaponStats.ClipSize;
            _sound = GetComponent<StudioEventEmitter>();
        }


        public async Task Reset(object sender)
        {
            canShoot = false;
            //PlayAnimation
            await Task.Delay(WeaponStats.ResetTime);
            canShoot = true;
        }

        public async Task Shoot(object sender, object shootableArgs)
        {
            if (shootableArgs is null) return;
            if(!CanShoot()) return;
            if (BulletsPerClip < 1)
            {
                //PlayClipEmptySound
                throw new ShootFailedException(ShootFailedException.WeaponErrorType.NoAmmoInClip);
            }

            if (BulletsLeft < 1 && BulletsPerClip < 1)
            {
                throw new ShootFailedException(ShootFailedException.WeaponErrorType.NoAmmo);
            }
            
            //DoShootLogic
            _animationController?.PlayAnimation(WeaponStats.ShootAnimation);
            weaponFire.Play();
            _sound.Event = WeaponStats.ShootEvent;
            _sound.Play();

            if (shootableArgs is GameObject cam && Physics.Raycast(cam.transform.position, cam.transform.forward, out var raycastHit, WeaponStats.Range, ~LayerMask))
            {

                if (raycastHit.collider.gameObject.layer != 14 && raycastHit.collider.gameObject.layer != 15)
                {
                    DecalManager.Instance.PlaceDefaultDecal(raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
                } //Si no es un pollo (o un enemigo)
                
                var gp = raycastHit.collider.gameObject.GetComponent<HealthSystem>() ?? raycastHit.collider.gameObject.GetComponent<DummyHealthSystem>();

                gp?.TakeDamage(WeaponStats);
            }
            
            
            BulletsPerClip--;

            await Task.Delay(WeaponStats.CycleTime);

            
        }

        public async Task Reload(object sender)
        {
            if (BulletsLeft < 1 || BulletsPerClip == WeaponStats.ClipSize)
            {
                return;
            }
            
            canShoot = false;
            isReloading = true;
            _animationController?.PlayAnimation(WeaponStats.ReloadAnimation);
            _sound.Event = WeaponStats.ReloadEvent;
            _sound.Play();
            await Task.Delay(WeaponStats.ReloadTime);
            

            if (BulletsPerClip >= WeaponStats.ClipSize)
            {
                isReloading = false;
                canShoot = true;
                return;
            }

            

            var bulletsLeft = this.BulletsLeft + BulletsPerClip;
            var bulletsFromClip = BulletsPerClip;

            if (bulletsLeft <= WeaponStats.ClipSize)
            {
                this.BulletsPerClip = bulletsLeft;
                this.BulletsLeft = 0;
            }
            else
            {
                this.BulletsLeft = bulletsLeft - WeaponStats.ClipSize;
                this.BulletsPerClip = WeaponStats.ClipSize;
            }

            isReloading = false;
            canShoot = true;
        }


        public void RefillAmmo(int ammo)
        {
            BulletsLeft += ammo;
        }
    }
}