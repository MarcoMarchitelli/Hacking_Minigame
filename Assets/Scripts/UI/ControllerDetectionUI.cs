using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ControllerDetectionUI : MonoBehaviour
{
    [Header("UI References")]
    public Image background;
    public TextMeshProUGUI text;
    public Image deviceStateImage;

    [Header("Parameters")]
    [Multiline]
    public string DisconnectedMessage;
    [Multiline]
    public string ConnectedMessage;
    public bool pauseOnDisconnection;

    [Header("Events")]
    public UnityEvent OnDisconnectionPopUp, OnConnectionPopDown;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        background.transform.localScale = Vector3.zero;
        text.transform.localScale = Vector3.zero;
        deviceStateImage.transform.localScale = Vector3.zero;
    }

    public void DisconnectionPopUp()
    {
        text.text = DisconnectedMessage;
        if (pauseOnDisconnection)
            Time.timeScale = 0;

        Sequence popUp = DOTween.Sequence();
        popUp.SetUpdate(true);
        popUp.Append(background.transform.DOScale(1, .7f).SetEase(Ease.OutElastic));
        popUp.Append(deviceStateImage.transform.DOScale(1, .4f).SetEase(Ease.OutElastic));
        popUp.Append(text.transform.DOScale(1, .4f).SetEase(Ease.OutElastic));

        popUp.onComplete += () =>
        {
            deviceStateImage.transform.DOPunchScale(Vector3.one * 1.1f, .5f, 0, 1).SetLoops(-1).SetUpdate(true);
            deviceStateImage.transform.DOPunchRotation(Vector3.forward * 30, .5f, 10, 1).SetLoops(-1).SetUpdate(true);
        };

        OnDisconnectionPopUp.Invoke();
    }

    public void ConnectionPopDown()
    {
        text.text = ConnectedMessage;

        deviceStateImage.transform.DOKill();

        Sequence popDown = DOTween.Sequence();
        popDown.SetUpdate(true);
        popDown.Append(text.transform.DOScale(0, .4f).SetEase(Ease.InElastic));
        popDown.Append(deviceStateImage.transform.DOScale(0, .4f).SetEase(Ease.InElastic));
        popDown.Append(background.transform.DOScale(0, .7f).SetEase(Ease.InElastic));
        popDown.onComplete += () =>
        {
            if (pauseOnDisconnection)
                Time.timeScale = 1;
            OnConnectionPopDown.Invoke();
        };
    }
}