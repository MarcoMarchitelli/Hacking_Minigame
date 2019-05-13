using UnityEngine;

public class Projectile : MonoBehaviour, IDestructable
{
    public LayerMask damageMask;
    public LayerMask destructionMask;
    public float damage = 1;

    [Header("Graphics References")]
    public ParticleSystem particle_Enemy_Hit;
    //public Rewinder rewinder;

    [HideInInspector] public float moveSpeed;

    const float LIFE_TIME = 3f;

    bool canMove = true;

    //private void Awake()
    //{
    //    RewindManager.Instance.OnRewindStart += StopMove;
    //    RewindManager.Instance.OnRewindEnd += StartMove;
    //    if (rewinder)
    //        rewinder.spawnPoint = transform.position;
    //}

    private void Start()
    {
        Die(LIFE_TIME);
    }

    void Update()
    {
        //if (canMove)

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageMask == (damageMask | (1 << other.gameObject.layer)))
        {
            IDamageable d = other.GetComponentInParent<IDamageable>();

            if (d != null)
            {
                d.TakeDamage(damage);
                if (particle_Enemy_Hit)
                    Instantiate(particle_Enemy_Hit.gameObject, transform.position, Quaternion.identity);
            }
        }
        
        if (destructionMask == (destructionMask | (1 << other.gameObject.layer)))
        {
            Die();
        }
    }

    void Die(float _delay)
    {
        Destroy(gameObject, _delay);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    //void StartMove()
    //{
    //    canMove = true;
    //}

    //void StopMove()
    //{
    //    canMove = false;
    //}
}