using Interfaces;
using JetBrains.Annotations;

namespace Utils
{
    public class InventorySystem
    {
        private IShootable[] _weapons = new IShootable[7];
        public enum WeaponSlot
        {
            MainWeapon, SecondaryWeapon, Knife, GrenadeSlot
        }
        
        private ushort index;

        private bool TryGet(WeaponSlot slot, [CanBeNull] out IShootable weapon)
        {
            weapon = null;
            return true;
        }

        private bool TryGet(ushort indexnum, [CanBeNull] out IShootable weapon)
        {
            if (_weapons[indexnum] != null)
            {
                index = indexnum;
                weapon = _weapons[index];
                return true;
            }

            weapon = null;
            return false;
        }

        private bool TrySet(IShootable weapon)
        {
            return true;
        }
    }
}