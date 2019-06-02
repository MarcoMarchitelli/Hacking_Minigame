using UnityEngine;
using DG.Tweening;

public class Scaler : MonoBehaviour
{
    public bool scaleOnStart = false;
    public float duration;
    public Vector3 endScale;

    private void Start()
    {
        if (scaleOnStart)
            StartScaling();
    }

    public void StartScaling()
    {
        transform.DOScale(endScale, duration);
    }
}