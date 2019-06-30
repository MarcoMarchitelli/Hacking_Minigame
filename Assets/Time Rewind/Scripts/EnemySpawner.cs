namespace Rewind
{
    using UnityEngine;
    using System.Collections;

    public class EnemySpawner : MonoBehaviour
    {
        public static int ScoreCount;

        public bool spawnOnStart;
        public GameObject[] prefabs;
        public float startTime, endTime;
        public AnimationCurve difficultyOverTime;
        public float width;

        private float timer, currentSpawnTime, gameLifeTime;
        private bool countLifeTime;

        private void Start()
        {
            RewindManager.Instance.OnRewindStart += StopSpawning;
            RewindManager.Instance.OnRewindEnd += StartSpawning;
            ScoreCount = 0;

            if (spawnOnStart)
            {
                StartSpawning();
            }
        }

        private void Update()
        {
            if (countLifeTime)
                gameLifeTime += Time.deltaTime;
        }

        void StartSpawning()
        {
            countLifeTime = true;
            currentSpawnTime = Mathf.Lerp(startTime, endTime, difficultyOverTime.Evaluate(gameLifeTime));
            StartCoroutine("SpawnRoutine");
        }

        void StopSpawning()
        {
            countLifeTime = false;
            StopCoroutine("SpawnRoutine");
        }

        IEnumerator SpawnRoutine()
        {
            timer = 0;
            while (timer < currentSpawnTime)
            {
                currentSpawnTime = Mathf.Lerp(startTime, endTime, difficultyOverTime.Evaluate(gameLifeTime));
                timer += Time.deltaTime;
                yield return null;
            }

            Spawn();
        }

        void Spawn()
        {
            StopSpawning();

            int randomIndex = Random.Range(0, prefabs.Length);
            Vector3 randomPos = new Vector3(Random.Range(transform.position.x - width * .5f, transform.position.x + width * .5f), transform.position.y, transform.position.z);

            RewindEnemy tempEnemy = Instantiate(prefabs[randomIndex], randomPos, transform.rotation).GetComponent<RewindEnemy>();
            tempEnemy.OnDeath += UpdateScoreCount;

            StartSpawning();
        }

        private void UpdateScoreCount()
        {
            ScoreCount++;
        }
    }
}