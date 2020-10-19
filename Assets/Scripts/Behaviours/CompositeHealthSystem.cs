using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class CompositeHealthSystem : HealthSystem
    {
        [Obsolete]
        public new void TakeDamage(WeaponStats stats)
        {
        }

        [SerializeField]
        private List<DummyHealthSystem> _hsList;

        new void Start()
        {
            base.Start();
            _hsList.ForEach(hs => hs.Constructor(this));
        }

        public void TakeDamage(WeaponStats stats, float modifier)
        {
            if (Armor > 0)
            {
                var armorHit = stats.Damage * 0.75 * modifier;
                var healthHit = stats.Damage * 0.25 * modifier;

                Armor = (int) (Armor - armorHit < 0 ? 0 : (Armor - armorHit));

                Health = (int) (Health - healthHit);
            }
            else
            {
                Health = (int) (Health - stats.Damage * modifier < -1 ? -1 : (Health - stats.Damage * modifier));
            }

            if (Health < 0)
            {
                _score.getPoints();
            }
        }
        
    }
}