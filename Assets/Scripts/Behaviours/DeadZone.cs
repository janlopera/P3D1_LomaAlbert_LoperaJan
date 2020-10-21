using System;
using UnityEngine;

namespace Behaviours
{
    public class DeadZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var hs =other.gameObject.GetComponent<HealthSystem>();
            var damages = ScriptableObject.CreateInstance<WeaponStats>();
            damages.Damage = 100000;
            hs?.TakeDamage(damages);
        }
    }
}