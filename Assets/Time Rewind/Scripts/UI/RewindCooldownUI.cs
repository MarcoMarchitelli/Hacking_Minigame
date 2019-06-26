namespace Rewind
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using DG.Tweening;

    public class RewindCooldownUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] TextMeshProUGUI cooldownText;
        [SerializeField] Image cooldownImage;

        private Vector3 textDefaultScale;
        private Color   textDefaultColor;
        private Vector3 imageDefaultScale;
        private RewindManager rewindManager;
        private bool update;

        private const float EFFECT_SCALE_MODIFIER = 1.7f;

        private void Start()
        {
            rewindManager = RewindManager.Instance;

            rewindManager.OnRewindEnd += StartCooldown;

            textDefaultColor  = cooldownText.color;
            textDefaultScale  = cooldownText.transform.localScale;
            imageDefaultScale = cooldownImage.transform.localScale;

            cooldownText.transform.localScale  = textDefaultScale * EFFECT_SCALE_MODIFIER;
            cooldownImage.transform.localScale = imageDefaultScale * EFFECT_SCALE_MODIFIER;
            cooldownText.color                 = Color.clear;
        }

        private void Update()
        {
            if (update)
                UpdateUI();
        }

        private void StartCooldown()
        {
            cooldownText.transform.DOScale(textDefaultScale, 1.5f).SetEase(Ease.OutCubic);
            cooldownText.DOColor(textDefaultColor, 1.5f).SetEase(Ease.OutCubic);
            cooldownImage.transform.DOScale(imageDefaultScale, 1.5f).SetEase(Ease.OutCubic);
            cooldownImage.material.SetFloat("__FillPercent", 0);
            cooldownImage.material.DOFloat(0, "_FillPercent", RewindManager.REWIND_TIME).onComplete += () => update = false;
            update = true;
        }

        private void UpdateUI()
        {
            cooldownText.text = rewindManager.timer.ToString();
        }
    } 
}