using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;

public class RewindModeTutorial : MonoBehaviour
{
    [Header("UI References")]
    public Image backgroundImage;
    public Image slideImage;
    public TextMeshProUGUI slideText;

    [Header("Slides")]
    public TutorialSlide[] tutorialSlides;

    bool readInput;
    int currentSlideIndex;
    TutorialSlide currentSlide;

    private void Awake()
    {
        slideImage.transform.localScale = Vector3.zero;
        slideText.transform.localScale = Vector3.zero;
    }

    public void StartTutorial()
    {
        backgroundImage.transform.DOScale(1, .7f).SetEase(Ease.OutElastic).onComplete += () => StartCoroutine("TutorialRoutine");
    }

    IEnumerator TutorialRoutine()
    {
        currentSlide = tutorialSlides[currentSlideIndex];

        slideImage.sprite = currentSlide.sprite;
        slideText.text    = currentSlide.text;

        Sequence slidePopUp = DOTween.Sequence();

        slidePopUp.Append(slideImage.transform.DOScale(1, .7f).SetEase(Ease.OutElastic));
        slidePopUp.Append(slideText.transform.DOScale(1, .5f).SetEase(Ease.OutElastic));

        while (true)
        {

            
            yield return null;
        }
    }

    public struct TutorialSlide
    {
        [Multiline]
        public string text;
        public Sprite sprite;
    }
}