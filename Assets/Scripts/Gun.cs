using UnityEngine;

public class Gun : MonoBehaviour
{
    public Projectile projectilePrefab;
    public Transform shootPoint;
    public float timeBetweenEachShot = .5f;
    public float projectileSpeed = 25f;
    public Vector3 projectileRotation;

    [Header("References")]
    public AudioSource ShotSource;
    public ParticleSystem ShotParticle;

    float timer = 0;

    private void Start()
    {
        timer = timeBetweenEachShot;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            timer = timeBetweenEachShot;
    }

    public void Shoot()
    {
        if (timer >= timeBetweenEachShot)
        {
            Projectile proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation) as Projectile;
            proj.moveSpeed = projectileSpeed;
            proj.transform.rotation = proj.transform.rotation * Quaternion.Euler(projectileRotation);
            timer = timeBetweenEachShot;
            if (ShotSource)
                ShotSource.Play();
            if (ShotParticle)
                Instantiate(ShotParticle, shootPoint.position, Quaternion.identity);
        }
    }
}