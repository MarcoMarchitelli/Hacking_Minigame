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
        else
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenEachShot)
                timer = 0;
        }
    }

    public void Shoot()
    {
        if (!canShoot)
            return;

        if (timer <= 0)
        {
            Projectile proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation) as Projectile;
            proj.moveSpeed = projectileSpeed;
            proj.transform.rotation = proj.transform.rotation * Quaternion.Euler(projectileRotation);
            timer = timeBetweenEachShot;
            if (ShotSource)
                ShotSource.Play();
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