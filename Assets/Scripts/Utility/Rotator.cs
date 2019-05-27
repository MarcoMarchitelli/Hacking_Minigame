using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 Euler;
    public float speed;

    void Update()
    {
        transform.Rotate(Euler * speed * Time.deltaTime, Space.Self);
    }
}