using UnityEngine;
using DG.Tweening;

/// <summary>
/// Handles Inputs and Generic Player Data.
/// </summary>
[RequireComponent(typeof(PlayerController_Infinite_Runner))]
public class Player_Infinite_Runner : LivingEntity
{
    #region Inspector Vars

    public float moveSpeed = 7;
    public Gun startingGun;

    [Header("Graphics References")]
    public GameObject ship_Side_L;
    public GameObject ship_Side_R;
    public ParticleSystem particle_Ship_Movement;
    public ParticleSystem particle_Ship_Damage;
    public ParticleSystem particle_Ship_Death;

    [Header("DEBUG MODE")]
    public bool DEBUG_MODE = false;

    #endregion

    PlayerController_Infinite_Runner controller;
    Gun equippedGun;
    ShipRenderer[] shipRenderers;

    #region Constants

    /// <summary>
    /// Interpolation time for the material to go from one color to another.
    /// </summary>
    const float FLASH_INTERPOLATION_DURATION = .1f;
    /// <summary>
    /// How much time to wait from the first color interpolation to the second.
    /// </summary>
    const float FLASH_WAIT_DURATION = .3f;
    const float INVULNERABILITY_DURATION = .7f;

    const float CAMERA_SHAKE_DURATION = .3f;
    const float CAMERA_SHAKE_FREQUENCY = 15f;
    const float CAMERA_SHAKE_AMPLITUDE = .5f;

    #endregion

    #region Monos

    private void Awake()
    {
        controller = GetComponent<PlayerController_Infinite_Runner>();

        //renderers color storing for material flash
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        shipRenderers = new ShipRenderer[renderers.Length];
        for (int i = 0; i < shipRenderers.Length; i++)
        {
            shipRenderers[i] = new ShipRenderer(renderers[i], renderers[i].material.GetColor("_BaseColor"), renderers[i].material.GetColor("_EmissionColor"));
        }
    }

    protected override void Start()
    {
        //health set
        base.Start();

        //gun check
        if (startingGun != null)
            equippedGun = startingGun;

        //death event subscriptions
        OnDeath += Die;

        //damage event subscriptions
        OnDamage += Damage;
    }

    void Update()
    {
        //debug inputs
        if (DEBUG_MODE)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnDamage?.Invoke();
            }
        }

        //movement input
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Left Stick Horizontal"), 0, Input.GetAxisRaw("Left Stick Vertical")).normalized;
        Vector3 moveVelocity = moveDirection * Time.deltaTime * moveSpeed;

        controller.Move(moveVelocity);

        //shoot input
        if(Input.GetButton("Infinite Runner Shoot"))
        {
            equippedGun.Shoot();
        }

        //movement particle
        if (particle_Ship_Movement.isPlaying && moveVelocity == Vector3.zero)
            particle_Ship_Movement.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        else if (particle_Ship_Movement.isStopped)
            particle_Ship_Movement.Play();
    }

    #endregion

    /// <summary>
    /// Damage Event default callback.
    /// </summary>
    void Damage()
    {
        switch (currentHealt)
        {
            case 2:
                ship_Side_L.SetActive(false);
                break;
            case 1:
                ship_Side_R.SetActive(false);
                break;
        }

        StartInvulnerability();

        DamageEffects();
    }

    /// <summary>
    /// Material flash and particle play.
    /// </summary>
    void DamageEffects()
    {
        CameraShake.Instance.Shake(CAMERA_SHAKE_AMPLITUDE, CAMERA_SHAKE_FREQUENCY, CAMERA_SHAKE_DURATION);

        particle_Ship_Damage.Play(true);

        //material flash sequence creation
        Sequence materialsFlash = DOTween.Sequence();
        for (int i = 0; i < shipRenderers.Length; i++ )
        {
            Sequence matFlash = DOTween.Sequence();

            matFlash.Append(shipRenderers[i].renderer.material.DOColor(Color.black, "_EmissionColor", FLASH_INTERPOLATION_DURATION));
            matFlash.Join(shipRenderers[i].renderer.material.DOColor(Color.black, "_BaseColor", FLASH_INTERPOLATION_DURATION));
            matFlash.AppendInterval(FLASH_WAIT_DURATION);
            matFlash.Append(shipRenderers[i].renderer.material.DOColor(shipRenderers[i].startingEmissionColor, "_EmissionColor", FLASH_INTERPOLATION_DURATION));
            matFlash.Join(shipRenderers[i].renderer.material.DOColor(shipRenderers[i].startingBaseColor, "_BaseColor", FLASH_INTERPOLATION_DURATION));

            materialsFlash.Join(matFlash);
        }
    }

    void StartInvulnerability()
    {
        invulnerable = true;
        Timer.StartTimer(INVULNERABILITY_DURATION, StopInvulnerability);
    }

    void StopInvulnerability()
    {
        invulnerable = false;
    }

    /// <summary>
    /// Death event default callback.
    /// </summary>
    public override void Die()
    {
        Instantiate(particle_Ship_Death.gameObject, transform.position, Quaternion.identity);

        Destroy(gameObject);

        //death event subscriptions
        OnDeath -= Die;

        //damage event subscriptions
        OnDamage -= Damage;
    }

    struct ShipRenderer
    {
        public MeshRenderer renderer;
        public Color startingBaseColor;
        public Color startingEmissionColor;

        public ShipRenderer(MeshRenderer renderer, Color startingBaseColor, Color startingEmissionColor)
        {
            this.renderer = renderer;
            this.startingBaseColor = startingBaseColor;
            this.startingEmissionColor = startingEmissionColor;
        }
    }
}