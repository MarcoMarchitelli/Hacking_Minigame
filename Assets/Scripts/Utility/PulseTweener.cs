using UnityEngine;
using DG.Tweening;

public class PulseTweener : MonoBehaviour
{
    private const float SCALE_MULTIPLIER = 2;
    private const float PULSE_DURATION = 1f;

    private void Start()
    {
        Sequence pulsatingSequence = DOTween.Sequence();

        pulsatingSequence.Append(transform.DOScale(SCALE_MULTIPLIER, PULSE_DURATION).SetEase(Ease.Linear));
        pulsatingSequence.Append(transform.DOScale(1, PULSE_DURATION).SetEase(Ease.Linear));
        pulsatingSequence.SetLoops(-1);
    }
}