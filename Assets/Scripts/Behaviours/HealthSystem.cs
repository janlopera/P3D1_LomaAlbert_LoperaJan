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

        protected void Start()
        {
            _score = GetComponent<ScoreObject>();
        }


        public void TakeDamage(WeaponStats stats)
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

            if (Health < 0)
            {
                _score.getPoints();
            }
        }

        public void RefillHealth(int i)
        {
            Health = Health + i > MAX_HEALTH ? MAX_HEALTH : Health + i;
        }
        public void RefillArmor(int i)
        {
            Armor = Armor + i > MAX_ARMOR ? MAX_ARMOR : Armor + i;
        }
        
    }
}