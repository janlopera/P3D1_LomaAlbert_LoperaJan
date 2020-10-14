using System;

namespace Models.Exceptions
{
    public class ShootFailedException : Exception
    {
        public enum WeaponErrorType{NoAmmoInClip, NoAmmo};

        public WeaponErrorType ErrorType;
        public ShootFailedException(WeaponErrorType weaponError)
        {
            ErrorType = weaponError;
        }
    }
}