using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class ButtonUI : MonoBehaviour
{
    public UnityEvent OnSelection;

    Button button;
    TextMeshProUGUI text;

    Vector3 originalScale;
    Color originalColor;
    bool selected;

    const float SELECTION_SCALE_MULTIPLIER = 1.3f;
    const float CLICK_SCALE_MULTIPLIER = 1.5f;
    const float SELECTION_ANIMATION_DURATION = .3f;
    const float CLICK_ANIMATION_DURATION = .2f;

    #region API
    public void Setup()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        originalScale = transform.localScale;
        originalColor = button.image.color;
        FadeImage(false, 0);
    }

    public void Select()
    {
        button.image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        FadeImage(true, .1f);
        transform.DOPunchScale(originalScale * SELECTION_SCALE_MULTIPLIER, SELECTION_ANIMATION_DURATION, 0, .7f);
        selected = true;
        OnSelection.Invoke();
    }

    public void Deselect()
    {
        FadeImage(false, .1f);
        transform.DOScale(originalScale, SELECTION_ANIMATION_DURATION);
        selected = false;
    }

    public void Click()
    {
        button.image.DOColor(Color.cyan, CLICK_ANIMATION_DURATION).SetEase(Ease.OutElastic);
        transform.DOPunchScale(originalScale * CLICK_SCALE_MULTIPLIER, CLICK_ANIMATION_DURATION, 0, .7f);
        transform.DOScale(originalScale * SELECTION_SCALE_MULTIPLIER, CLICK_ANIMATION_DURATION);
        button.onClick.Invoke();
    }

    public void FadeAll(bool _fadeValue, float _duration, System.Action _callback = null)
    {
        if (selected)
            button.image.DOFade(_fadeValue == true ? 1 : 0, _duration);
        text.DOFade(_fadeValue == true ? 1 : 0, _duration).onComplete += () => _callback?.Invoke();
    }

    public void FadeImage(bool _fadeValue, float _duration, System.Action _callback = null)
    {
        button.image.DOFade(_fadeValue == true ? 1 : 0, _duration).onComplete += () => _callback?.Invoke();
    }
    #endregion
}