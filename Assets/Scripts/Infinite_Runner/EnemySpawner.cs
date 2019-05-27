using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public float minTime, maxTime;
    public float width;

    private void Start()
    {
        Timer.StartTimer(Random.Range(minTime, maxTime), Spawn);
    }

    void Spawn()
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        Vector3 randomPos = new Vector3(Random.Range(transform.position.x - width * .5f, transform.position.x + width * .5f), transform.position.y, transform.position.z);

        Instantiate(prefabs[randomIndex], randomPos, transform.rotation);

        Timer.StartTimer(Random.Range(minTime, maxTime), Spawn);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 5, 1));
    }
}