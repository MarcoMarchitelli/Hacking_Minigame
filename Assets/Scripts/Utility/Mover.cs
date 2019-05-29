using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public bool moveOnStart;
    public Vector3 translation;
    public Space space;

    void Start()
    {
        if (moveOnStart)
        {
            StartMove();
        }

        RewindManager.Instance.OnRewindStart += StopMove;
        RewindManager.Instance.OnRewindEnd += StartMove;
    }

    private void OnDisable()
    {
        RewindManager.Instance.OnRewindStart -= StopMove;
        RewindManager.Instance.OnRewindEnd -= StartMove;
    }

    void StartMove()
    {
        StartCoroutine("MoveRoutine");
    }

    void StopMove()
    {
        StopCoroutine("MoveRoutine");
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            transform.Translate(translation * Time.deltaTime, space);

            yield return null;
        }
    }
}