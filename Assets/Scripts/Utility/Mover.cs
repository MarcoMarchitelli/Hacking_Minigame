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
            Move(true);
        }
    }

    public void Move(bool _value)
    {
        if (_value)
        {
            StartCoroutine("MoveRoutine");
        }
        else
        {
            StopCoroutine("MoveRoutine");
        }
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