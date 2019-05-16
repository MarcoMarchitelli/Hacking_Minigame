using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : LivingEntity
{
    #region Inspector Vars

    public LayerMask damageMask;
    public float damage;
    public Gun[] equippedGuns;

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

    Rigidbody rb;
    Material mat;
    Color startingColor;

    #region Monos

    private void Awake()
    {
        EnemiesCounter e = FindObjectOfType<EnemiesCounter>();
        if (e)
        {
            e.AddCount();
            OnDeath += e.RemoveCount;
        }

        rb = GetComponent<Rigidbody>();
        if (materialRenderer)
        {
            mat = materialRenderer.material;
            startingColor = mat.color;
        }
    }

    protected override void Start()
    {
        base.Start();

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
    }

    #endregion

    void Shoot()
    {
        foreach (Gun gun in equippedGuns)
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
}