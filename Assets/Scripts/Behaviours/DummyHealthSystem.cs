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

        public override void TakeDamage(WeaponStats stats)
        {
            hs.TakeDamage(stats, DamageModifier);
        }
    }
}