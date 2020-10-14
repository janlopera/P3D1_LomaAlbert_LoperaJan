using System;
using System.Threading.Tasks;
using Interfaces;
using Models.Exceptions;

namespace Models.Weapons
{
    public class Weapon : IShootable
    {
        private float Price;
        private float Damage;
        private float RangeModifier;
        private float CycleTime;
        private float Penetration;
        private float KillAward;
        private float MaxPlayerSpeed;
        private int ClipSize;
        private float Range;
        private bool IsFullAuto;
        private int BulletsPerShot;
        private int ReserveAmo;

        private int BulletsPerClip;
        private int BulletsLeft;

        private int ResetTime;
        private int ReloadTime;
        private bool canShoot = false;

        private bool CanShoot()
        {
            return canShoot;
        }

        public bool isAuto()
        {
            return IsFullAuto;
        }


        public async Task Reset(object sender)
        {
            canShoot = false;
            //PlayAnimation
            await Task.Delay(ResetTime);
            canShoot = true;

        }

        public async Task Shoot(object sender, object shootableArgs)
        {
            if (CanShoot())
            {
                //DoShootLogic
                
                
                await Task.Delay(TimeSpan.FromSeconds(CycleTime));
            }
            else
            {
                if (BulletsPerClip < 1)
                {
                    //PlayClipEmptySound
                    
                    throw new ShootFailedException(ShootFailedException.WeaponErrorType.NoAmmoInClip);
                }
                if (BulletsLeft < 1 && BulletsPerClip < 1)
                {
                    throw new ShootFailedException(ShootFailedException.WeaponErrorType.NoAmmo);
                }
            }
        }

        public async Task Reload(object sender)
        {
            canShoot = false;
            await Task.Delay(ReloadTime);

            if (BulletsPerClip >= ClipSize)
            {
                return;
            }

            if (BulletsLeft < 1)
            {
                return;
            }
            
            var bulletsLeft = this.BulletsLeft + BulletsPerClip;
            var bulletsFromClip = BulletsPerClip;

            if (bulletsLeft <= ClipSize)
            {
                this.BulletsPerClip = bulletsLeft;
                this.BulletsLeft = 0;
            }
            else
            {
                this.BulletsLeft = ClipSize - bulletsFromClip;
                this.BulletsPerClip = ClipSize;
            }

            canShoot = true;
        }
    }
}