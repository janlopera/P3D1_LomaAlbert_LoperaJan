using Controllers;
using FMODUnity;
using UnityEngine;

namespace Behaviours
{
    
    [RequireComponent(typeof(ScoreObject))]
    public class HealthSystem : MonoBehaviour
    {
        public int Health = 0;
        public int Armor = 0;

        public int MAX_HEALTH;
        public int MAX_ARMOR;

        protected ScoreObject _score;

        [SerializeField] private bool isPlayer = false;
        
        [SerializeField]
        private HUDController _hudController;

        private StudioEventEmitter _sound;

        private bool isDead = false;

        protected void Start()
        {
            _score = GetComponent<ScoreObject>();
            _hudController = GetComponent<HUDController>();
            _hudController?.UpdateHS(Health, Armor);
            _sound = GetComponent<StudioEventEmitter>();
        }


        public virtual void TakeDamage(WeaponStats stats)
        {
            if (Armor > 0)
            {
                var armorHit = stats.Damage * 0.75;
                var healthHit = stats.Damage * 0.25;

                Armor = (int) (Armor - armorHit < 0 ? 0 : (Armor - armorHit));

                Health = (int) (Health - healthHit);
            }
            else
            {
                Health = (int) (Health - stats.Damage < -1 ? -1 : (Health - stats.Damage));
            }
            
            _hudController?.UpdateHS(Health, Armor);

            if (Health >= 0) return;
            _hudController?.UpdateHS(Health, Armor);
            _score.getPoints();

            
            
            if (!isPlayer || isDead) return;
            isDead = true;
            _sound.Event = "event:/Character/Dead";
            _sound.Play();
            _hudController?.GameOver();
        }

        public void RefillHealth(int i)
        {
            Health = Health + i > MAX_HEALTH ? MAX_HEALTH : Health + i;
            _hudController?.UpdateHS(Health, Armor);
        }
        public void RefillArmor(int i)
        {
            Armor = Armor + i > MAX_ARMOR ? MAX_ARMOR : Armor + i;
            _hudController?.UpdateHS(Health, Armor);
        }
        
    }
}