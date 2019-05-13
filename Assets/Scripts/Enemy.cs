﻿using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Enemy : LivingEntity
{
    #region Inspector Vars

    public LayerMask damageMask;
    public float damage;
    public Gun[] equippedGuns;

    #endregion

    #region Constants

    const float FLASH_INTERPOLATION_DURATION = .05f;
    const float FLASH_WAIT_DURATION = .05f;

    #endregion

    Rigidbody rb;
    Material mat;
    Color startingColor;

    #region Monos

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;
        startingColor = mat.color;
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
        Destroy(gameObject);

        //death event subscriptions
        OnDeath -= Die;

        //damage event subscriptions
        OnDamage -= Damage;
    }

    void Damage()
    {
        //material flash sequence
        Sequence materialFlash = DOTween.Sequence();
        materialFlash.Append(mat.DOColor(Color.white, "_EmissionColor", FLASH_INTERPOLATION_DURATION));
        materialFlash.AppendInterval(FLASH_WAIT_DURATION);
        materialFlash.Append(mat.DOColor(startingColor, "_EmissionColor", FLASH_INTERPOLATION_DURATION));
    }
}