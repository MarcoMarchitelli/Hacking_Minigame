using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewindUI : MonoBehaviour
{
    public float fadeTime = 1.5f;

    public Slider slider;
    public Image sliderBackgroundImage;
    public Image sliderFillImage;

    Color sliderBackgroundImageColor;
    Color sliderFillImageColor;

    RewindManager rewind;

    bool update;

    private void Awake()
    {
        sliderBackgroundImageColor = sliderBackgroundImage.color;
        sliderFillImageColor = sliderFillImage.color;

        sliderFillImage.color = Color.clear;
        sliderBackgroundImage.color = Color.clear;
    }

    private void Start()
    {
        rewind = RewindManager.Instance;
        rewind.OnRewindStart += FadeIn;
        rewind.OnRewindEnd += FadeOut;
        slider.maxValue = slider.value = RewindManager.REWIND_TIME;
    }

    private void Update()
    {
        if(update)
            UpdateUI();
    }

    void UpdateUI()
    {
        slider.value = slider.maxValue - rewind.timer;
    }

    void FadeIn()
    {
        update = true;
        sliderBackgroundImage.DOColor(sliderBackgroundImageColor, fadeTime);
        sliderFillImage.DOColor(sliderFillImageColor, fadeTime);
    }

    void FadeOut()
    {
        update = false;
        sliderBackgroundImage.DOColor(Color.clear, fadeTime);
        sliderFillImage.DOColor(Color.clear, fadeTime);
    }
}