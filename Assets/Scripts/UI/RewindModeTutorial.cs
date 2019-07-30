using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;

public class RewindModeTutorial : MonoBehaviour
{
    [Header("UI References")]
    public Image backgroundImage;
    public TextMeshProUGUI nextText;
    public Image AButtonImage;

    [Header("Slides")]
    public GameObject[] tutorialSlides;

    public UnityEvent OnTutorialStart, OnTutorialEnd;

    int currentSlideIndex;
    GameObject currentSlide;

    private void Awake()
    {
        foreach (GameObject slide in tutorialSlides)
        {
            slide.transform.localScale = Vector3.zero;
        }

        StartTutorial();
    }

    public void StartTutorial()
    {
        OnTutorialStart.Invoke();

        currentSlideIndex = -1;

        Sequence tutorialStartSequence = DOTween.Sequence();
        tutorialStartSequence.Append(backgroundImage.transform.DOScale(1, .7f).SetEase(Ease.OutElastic));
        tutorialStartSequence.Insert(.3f, AButtonImage.transform.DOScale(1, .4f).SetEase(Ease.OutElastic));
        tutorialStartSequence.Insert(.5f, nextText.transform.DOScale(1, .4f).SetEase(Ease.OutElastic));
        tutorialStartSequence.onComplete += () => StartCoroutine("TutorialRoutine");
    }

    IEnumerator TutorialRoutine()
    {
        currentSlideIndex++;
        if (currentSlideIndex > tutorialSlides.Length - 1)
        {
            EndTutorial();
            yield break;
        }
        currentSlide = tutorialSlides[currentSlideIndex];

        currentSlide.transform.DOScale(1, .5f).SetEase(Ease.OutElastic);

        while (true)
        {
            if (Input.GetButtonDown("Confirm"))
            {
                currentSlide.transform.DOScale(0, .3f).SetEase(Ease.InElastic).onComplete += () => StartCoroutine("TutorialRoutine");
                yield break;
            }

            yield return null;
        }
    }

    void EndTutorial()
    {
        Sequence tutorialEndSequence = DOTween.Sequence();
        tutorialEndSequence.Append(nextText.transform.DOScale(0, .4f).SetEase(Ease.InElastic));
        tutorialEndSequence.Insert(.1f, AButtonImage.transform.DOScale(0, .4f).SetEase(Ease.InElastic));
        tutorialEndSequence.Insert(.2f, backgroundImage.transform.DOScale(0, .5f).SetEase(Ease.InElastic));

        tutorialEndSequence.onComplete += () => OnTutorialEnd.Invoke();
    }
}