using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool spawnOnStart;
    public GameObject[] prefabs;
    public float startTime, endTime;
    public AnimationCurve difficultyOverTime;
    public float width;

    bool countTime;
    float timer, currentSpawnTime;

    private void Start()
    {
        if (spawnOnStart)
        {
            StartSpawning();
        }

        RewindManager.Instance.OnRewindStart += StopSpawning;
        RewindManager.Instance.OnRewindEnd += StartSpawning;
    }

    private void Update()
    {
        currentSpawnTime = Mathf.Lerp(startTime, endTime, difficultyOverTime.Evaluate(Time.time));

        if (countTime)        
            timer += Time.deltaTime;        
        else
            timer -= Time.deltaTime;

        if (timer <= 0)
            timer = currentSpawnTime;

        if (timer > currentSpawnTime)
            Spawn();
    }

    void StartSpawning()
    {
        timer = 0;
        countTime = true;
    }

    void StopSpawning()
    {
        countTime = false;
    }

    void Spawn()
    {
        StopSpawning();

        int randomIndex = Random.Range(0, prefabs.Length);
        Vector3 randomPos = new Vector3(Random.Range(transform.position.x - width * .5f, transform.position.x + width * .5f), transform.position.y, transform.position.z);

        Instantiate(prefabs[randomIndex], randomPos, transform.rotation);

        StartSpawning();
    }
}