public interface IDamageable
{
    float startingHealth { get; }

    void TakeDamage(float damage);
}