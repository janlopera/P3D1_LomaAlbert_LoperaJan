using System;
using Models.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class HUDController : MonoBehaviour
    {
        public Weapon currentWeapon;

        public TextMeshProUGUI clipAmmo, reserveAmmo;
        
        private void LateUpdate()
        {
            clipAmmo.text = $"{currentWeapon.BulletsPerClip}/";
            reserveAmmo.text = $"{currentWeapon.BulletsLeft}";
        }
    }
}