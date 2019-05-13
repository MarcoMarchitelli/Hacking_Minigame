using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable, IDestructable
{
    [Header("Statistics")]
    public float startingHealth;

    protected float currentHealt;

    protected bool invulnerable;

    public System.Action OnDeath;
    public System.Action OnDamage;

    float IDamageable.startingHealth { get { return startingHealth; }}
     
    protected virtual void Start()
    {
        currentHealt = startingHealth;
    }

    public virtual void TakeDamage(float _amount)
    {
        if (invulnerable)
            return;

        currentHealt -= _amount;

        OnDamage?.Invoke();

        if(currentHealt <= 0)
            OnDeath?.Invoke();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}