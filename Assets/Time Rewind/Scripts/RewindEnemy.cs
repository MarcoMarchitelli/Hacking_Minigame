using UnityEngine;
using DG.Tweening;

namespace Rewind
{
    [RequireComponent(typeof(Rigidbody))]
    public class RewindEnemy : LivingEntity
    {
        #region Inspector Vars

        public LayerMask damageMask;
        public float damage;
        public RewindGun[] equippedGuns;

        [Header("References")]
        public Renderer materialRenderer;

        #endregion

        #region Constants

        const float FLASH_INTERPOLATION_DURATION = .05f;
        const float FLASH_WAIT_DURATION = .05f;

        const float CAMERA_SHAKE_DURATION = .1f;
        const float CAMERA_SHAKE_FREQUENCY = 15f;
        const float CAMERA_SHAKE_AMPLITUDE = .3f;

        #endregion

        Material mat;
        Color startingColor;
        bool countTime;
        float timer;
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
                if (value != _hidden)
                {
                    _hidden = value;
                    Desappear(!_hidden);
                }
            }
        }

        #region Monos

        private void Awake()
        {
            EnemiesCounter e = FindObjectOfType<EnemiesCounter>();
            if (e)
            {
                e.AddCount();
                OnDeath += e.RemoveCount;
            }

            if (materialRenderer)
            {
                mat = materialRenderer.material;
                startingColor = mat.color;
            }
        }

        protected override void Start()
        {
            base.Start();

            RewindManager.Instance.OnRewindStart += StopCounting;
            RewindManager.Instance.OnRewindEnd += StartCounting;

            StartCounting();

            //death event subscriptions
            OnDeath += Die;

            //damage event subscriptions
            OnDamage += Damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (damageMask == (damageMask | (1 << other.gameObject.layer)))
            {
                LivingEntity le = other.GetComponentInParent<LivingEntity>();

                if (le)
                {
                    le.TakeDamage(damage);
                    Die();
                }
            }
        }

        void Update()
        {
            if (equippedGuns != null)
                Shoot();

            if (countTime)
                timer += Time.deltaTime;
            else
            {
                float tempLifeTimer = timer - RewindManager.Instance.timer;
                if (!Hidden && tempLifeTimer <= 0)
                    Hidden = true;
                else if (Hidden && tempLifeTimer > 0)
                    Hidden = false;
            }
        }

        #endregion

        void Shoot()
        {
            foreach (RewindGun gun in equippedGuns)
            {
                gun.Shoot();
            }
        }

        public override void Die()
        {
            CameraShake.Instance.Shake(CAMERA_SHAKE_AMPLITUDE, CAMERA_SHAKE_FREQUENCY, CAMERA_SHAKE_DURATION);

            Destroy(gameObject);

            //death event subscriptions
            OnDeath -= Die;

            //damage event subscriptions
            OnDamage -= Damage;
        }

        void Damage()
        {
            //material flash sequence
            if (mat != null)
            {
                Sequence materialFlash = DOTween.Sequence();
                materialFlash.Append(mat.DOColor(Color.white, "_EmissionColor", FLASH_INTERPOLATION_DURATION));
                materialFlash.AppendInterval(FLASH_WAIT_DURATION);
                materialFlash.Append(mat.DOColor(startingColor, "_EmissionColor", FLASH_INTERPOLATION_DURATION));
            }
        }

        void StartCounting()
        {
            countTime = true;
        }

        void StopCounting()
        {
            countTime = false;
        }

        private void Desappear(bool _value)
        {
            //TODO: add effects or something => maybe sounds.            
            materialRenderer.enabled = _value;
        }
    } 
}