using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public bool rotateOnStart = true;
    public Vector3 Euler;
    public float speed;

    private void Start()
    {
        if (rotateOnStart)
            StartRotation();
    }

    void StartRotation()
    {
        StartCoroutine("Rotation");
    }
    void StopRotation()
    {
        StopCoroutine("Rotation");
    }

    IEnumerator Rotation()
    {
        while (true)
        {
            transform.Rotate(Euler * speed * Time.deltaTime, Space.Self);
            yield return null;
        }
    }
}