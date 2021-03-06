﻿using UnityEngine;

namespace Rewind
{
    public class RewindGun : MonoBehaviour
    {
        public RewindProjectile projectilePrefab;
        public Transform shootPoint;
        public float timeBetweenEachShot = .5f;
        public float projectileSpeed = 25f;
        public Vector3 projectileRotation;

        [Header("References")]
        public AudioSource ShotSource;
        public ParticleSystem ShotParticle;

        float timer = 0;
        bool canShoot = true;

        private void Start()
        {
            RewindManager.Instance.OnRewindStart += EndShooting;
            RewindManager.Instance.OnRewindEnd += StartShooting;
        }

        private void Update()
        {
            if (canShoot)
                timer -= Time.deltaTime;
            //else
            //{
            //    timer += Time.deltaTime;
            //    if (timer >= timeBetweenEachShot)
            //        timer = 0;
            //}
        }

        public void Shoot()
        {
            if (!canShoot)
                return;

            if (timer <= 0)
            {
                RewindProjectile proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation) as RewindProjectile;
                proj.moveSpeed = projectileSpeed;
                proj.transform.rotation = transform.rotation * Quaternion.Euler(projectileRotation);
                timer = timeBetweenEachShot;
                if (ShotSource)
                    ShotSource.Play();
                if (ShotParticle)
                    Instantiate(ShotParticle, shootPoint.position - Vector3.forward * 2f, Quaternion.identity);
            }
        }

        void StartShooting()
        {
            canShoot = true;
        }

        void EndShooting()
        {
            canShoot = false;
        }
    } 
}