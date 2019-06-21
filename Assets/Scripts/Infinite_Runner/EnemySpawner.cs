namespace Rewind
{
    using UnityEngine;
    using System.Collections;

    public class EnemySpawner : MonoBehaviour
    {
        public bool spawnOnStart;
        public GameObject[] prefabs;
        public float startTime, endTime;
        public AnimationCurve difficultyOverTime;
        public float width;

        float timer, currentSpawnTime;

        private void Start()
        {
            RewindManager.Instance.OnRewindStart += StopSpawning;
            RewindManager.Instance.OnRewindEnd   += StartSpawning;

            if (spawnOnStart)
            {
                StartSpawning();
            }
        }

        void StartSpawning()
        {
            currentSpawnTime = Mathf.Lerp(startTime, endTime, difficultyOverTime.Evaluate(Time.time));
            StartCoroutine("SpawnRoutine");
        }

        void StopSpawning()
        {
            StopCoroutine("SpawnRoutine");
        }

        IEnumerator SpawnRoutine()
        {
            timer = 0;
            while (timer < currentSpawnTime)
            {
                currentSpawnTime = Mathf.Lerp(startTime, endTime, difficultyOverTime.Evaluate(Time.time));
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

            Instantiate(prefabs[randomIndex], randomPos, transform.rotation);

            StartSpawning();
        }
    }
}