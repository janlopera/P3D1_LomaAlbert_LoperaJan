using System;
using System.Threading.Tasks;
using Interfaces;
using Models.Exceptions;

namespace Models.Weapons
{
    public class Weapon : IShootable
    {
        public float Price;
        public float Damage;
        public float RangeModifier;
        public float CycleTime;
        public float Penetration;
        public float KillAward;
        public float MaxPlayerSpeed;
        public int ClipSize;
        public float Range;
        public bool IsFullAuto;
        public int BulletsPerShot;
        public int ReserveAmo;

        public int BulletsPerClip;
        public int BulletsLeft;

        public int ResetTime;
        public int ReloadTime;
        public bool canShoot = false;

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