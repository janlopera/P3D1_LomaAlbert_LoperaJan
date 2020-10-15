using System;
using System.Threading.Tasks;
using Interfaces;
using Models.Exceptions;

namespace Models.Weapons
{
    public class Weapon : IShootable
    {
        public WeaponStats WeaponStats;

        public int BulletsPerClip;
        public int BulletsLeft;
        
        public bool canShoot = false;

        private bool CanShoot()
        {
            return canShoot;
        }

        public bool isAuto()
        {
            return WeaponStats.IsFullAuto;
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
            if (CanShoot())
            {
                //DoShootLogic
                
                
                await Task.Delay(TimeSpan.FromSeconds(WeaponStats.CycleTime));
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
            await Task.Delay(WeaponStats.ReloadTime);

            if (BulletsPerClip >= WeaponStats.ClipSize)
            {
                return;
            }

            if (BulletsLeft < 1)
            {
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
                this.BulletsLeft = WeaponStats.ClipSize - bulletsFromClip;
                this.BulletsPerClip = WeaponStats.ClipSize;
            }

            canShoot = true;
        }
    }
}