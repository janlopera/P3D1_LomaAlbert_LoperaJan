using System;
using Models.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    public class HUDController : MonoBehaviour
    {
        public Weapon currentWeapon;

        public TextMeshProUGUI clipAmmo, reserveAmmo, health, armor;

        private int _health, _armor;

        public GameObject GameOverCanvas;

        public Slider ArmorSlider;
        public Slider HealthSlider;
        
        

        private void LateUpdate()
        {
            clipAmmo.text = $"{currentWeapon.BulletsPerClip}/";
            reserveAmmo.text = $"{currentWeapon.BulletsLeft}";
            health.text = $"{_health}";
            HealthSlider.value = _health;
            armor.text = $"{_armor}";
            ArmorSlider.value = _armor;
        }

        public void UpdateHS(int health, int armor)
        {
            this._health = health;
            this._armor = armor;
        }

        public void GameOver()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            GameOverCanvas.SetActive(true);
        }

        public void Reset()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}