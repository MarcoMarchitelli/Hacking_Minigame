using UnityEngine;

public class Projectile : MonoBehaviour, IDestructable
{
    public LayerMask damageMask;
    public LayerMask destructionMask;
    public float damage = 1;
    public float lifeTime = 3f;

    [Header("Graphics References")]
    public ParticleSystem particle_Enemy_Hit;

    [HideInInspector] public float moveSpeed;

    float timer = 0;

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer >= lifeTime)
            Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageMask == (damageMask | (1 << other.gameObject.layer)))
        {
            IDamageable d = other.GetComponentInParent<IDamageable>();

            if (d != null)
                d.TakeDamage(damage);

        }

        if (destructionMask == (destructionMask | (1 << other.gameObject.layer)))
        {
            if (particle_Enemy_Hit)
            {
                if (other.gameObject.layer == 9 || other.gameObject.layer == 11)
                    Instantiate(particle_Enemy_Hit.gameObject, transform.position, Quaternion.identity);
            }

            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}