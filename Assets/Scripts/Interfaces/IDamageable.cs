namespace Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        void TakeDamage(float damage, float penetration);
        
    }
}