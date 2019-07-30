using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenFader : MonoBehaviour
{
    [Header("References")]
    public Image image;

    [Header("Parameters")]
    public float fadeTime = 1f;
    public float fadeInAlpha, fadeOutAlpha;

    public void FadeIn()
    {
        image.DOFade(fadeInAlpha, fadeTime);
    }

    public void FadeIn(float _targetAlpha)
    {
        image.DOFade(_targetAlpha, fadeTime);
    }

    public void FadeOut()
    {
        image.DOFade(fadeOutAlpha, fadeTime);
    }

    public void FadeOut(float _targetAlpha)
    {
        image.DOFade(_targetAlpha, fadeTime);
    }

    public void Fade(float _duration, float _targetAlpha)
    {
        image.DOFade(_targetAlpha, _duration);
    }
}