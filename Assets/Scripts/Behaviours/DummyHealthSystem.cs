namespace Behaviours
{
    public class DummyHealthSystem : HealthSystem
    {

        public CompositeHealthSystem hs;

        public float DamageModifier;

        public void Constructor(CompositeHealthSystem cs)
        {
            hs = cs;
        }

        public new void TakeDamage(WeaponStats stats)
        {
            hs.TakeDamage(stats, DamageModifier);
        }
    }
}