using UnityEngine;

namespace Rewind
{
    public class RewindProjectile : MonoBehaviour, IDestructable
    {
        public LayerMask damageMask;
        public LayerMask destructionMask;
        public float damage = 1;
        public float lifeTime = 3f;

        [Header("Graphics References")]
        public ParticleSystem particle_Enemy_Hit;
        public Rewinder rewinder;

        [HideInInspector] public float moveSpeed;

        private MeshRenderer meshRenderer;
        private float timer = 0;
        private bool canMove = true;
        private bool countTime = false;
        private bool _hidden;
        /// <summary>
        /// If we move time to BEFORE our spawn or AFTER our lifetime we desappear but not die.
        /// We die if, on rewind end, we are still hidden. 
        /// </summary>
        private bool Hidden
        {
            get { return _hidden; }
            set
            {
                if(value != _hidden)
                {
                    _hidden = value;
                    Desappear(!_hidden);
                }
            }
        }

        private void Start()
        {
            RewindManager.Instance.OnRewindStart += StopMove;
            RewindManager.Instance.OnRewindEnd += StartMove;

            meshRenderer = GetComponent<MeshRenderer>();

            timer = 0;
            countTime = true;
        }

        private void OnDisable()
        {
            RewindManager.Instance.OnRewindStart -= StopMove;
            RewindManager.Instance.OnRewindEnd -= StartMove;
        }

        void Update()
        {
            if (canMove)
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            if (countTime)
            {
                if (canMove)
                {
                    timer += Time.deltaTime;
                    if (timer >= lifeTime || timer <= 0)
                        Die();
                }
                else 
                {
                    float tempLifeTimer = timer - RewindManager.Instance.timer;
                    if (!Hidden && tempLifeTimer <= 0 || tempLifeTimer >= lifeTime)
                        Hidden = true;
                    else if (Hidden && tempLifeTimer > 0)
                        Hidden = false;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Hidden)
                return;

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

        void StartMove()
        {
            canMove = true;
        }

        void StopMove()
        {
            canMove = false;
        }

        private void Desappear(bool _value)
        {
            //TODO: add effects or something => maybe sounds.            
            meshRenderer.enabled = _value;
        }
    }
}