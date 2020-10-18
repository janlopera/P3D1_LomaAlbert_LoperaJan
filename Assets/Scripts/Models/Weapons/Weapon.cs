using System;
using System.Net.Configuration;
using System.Threading.Tasks;
using Interfaces;
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
        }

        public WeaponStats getStats()
        {
            return WeaponStats;
        }

        public void Start()
        {
            BulletsLeft = WeaponStats.ReserveAmo;
            BulletsPerClip = WeaponStats.ClipSize;
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
            if (!((bool) shootableArgs)) return;
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
            _animationController.PlayAnimation(WeaponStats.ShootAnimation);
            weaponFire.Play();
            BulletsPerClip--;

            await Task.Delay(WeaponStats.CycleTime);

            Debug.Log("PUM!");
        }

        public async Task Reload(object sender)
        {
            if (BulletsLeft < 1 || BulletsPerClip == WeaponStats.ClipSize)
            {
                return;
            }
            
            canShoot = false;
            isReloading = true;
            _animationController.PlayAnimation(WeaponStats.ReloadAnimation);
            await Task.Delay(WeaponStats.ReloadTime);
            Debug.Log("COVER ME!");

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
    }
}