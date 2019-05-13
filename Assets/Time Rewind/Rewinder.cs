using System.Collections.Generic;
using UnityEngine;

public class Rewinder : MonoBehaviour
{
    List<PointInTime> registeredPoints;

    bool isRewinding = false;
    [HideInInspector] public Vector3 spawnPoint;

    private void Awake()
    {
        RewindManager.Instance.AddRewinder(this);
        RewindManager.Instance.OnRewindStart += StartRewind;
        RewindManager.Instance.OnRewindEnd += EndRewind;

        registeredPoints = new List<PointInTime>();
    }

    private void OnDisable()
    {
        RewindManager.Instance.RemoveRewinder(this);
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
            if (transform.position == spawnPoint)
                Die();
        }
        else
            Register();
    }

    void Register()
    {
        if (registeredPoints.Count > Mathf.RoundToInt(RewindManager.REWIND_TIME / Time.fixedDeltaTime))
        {
            registeredPoints.RemoveAt(registeredPoints.Count - 1);
        }

        registeredPoints.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    void Rewind()
    {
        if (registeredPoints.Count > 0)
        {
            PointInTime temp = registeredPoints[0];
            transform.position = temp.position;
            transform.rotation = temp.rotation;
            registeredPoints.RemoveAt(0);
        }
        else
        {
            EndRewind();
        }
    }

    public void EndRewind()
    {
        isRewinding = false;
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    void Die()
    {
        Destroy(gameObject);
    }

}

public struct PointInTime
{
    public Vector3 position;
    public Quaternion rotation;

    public PointInTime(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}