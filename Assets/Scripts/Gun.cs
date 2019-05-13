﻿using UnityEngine;

public class Gun : MonoBehaviour
{
    public Projectile projectilePrefab;
    public Transform shootPoint;
    public float timeBetweenEachShot = .5f;
    public float projectileSpeed = 25f;
    public Vector3 projectileRotation;

    float timer = 0;
    //bool canShoot = true;

    //private void Start()
    //{
    //    RewindManager.Instance.OnRewindStart += EndShooting;
    //    RewindManager.Instance.OnRewindEnd += StartShooting;
    //}

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public void Shoot()
    {
        //if (!canShoot)
        //    return;

        if(timer <= 0)
        {
            Projectile proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation) as Projectile;
            proj.moveSpeed = projectileSpeed;
            proj.transform.rotation = proj.transform.rotation * Quaternion.Euler(projectileRotation);
            timer = timeBetweenEachShot;
        }
    }

    //void StartShooting()
    //{
    //    canShoot = true;
    //}
    //void EndShooting()
    //{
    //    canShoot = false;
    //}

}