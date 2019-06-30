namespace Rewind
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using DG.Tweening;

    public class RewindCooldownUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] TextMeshProUGUI cooldownText = null;
        [SerializeField] Image cooldownImage = null;

        private Vector3 textDefaultScale;
        private Vector3 imageDefaultScale;
        private RewindManager rewindManager;
        private bool update;
        private float timer;

        private const float EFFECT_SCALE_MODIFIER = 1.7f;

        private void Start()
        {
            rewindManager = RewindManager.Instance;

            rewindManager.OnRewindEnd += StartCooldown;

            textDefaultScale = cooldownText.transform.localScale;
            imageDefaultScale = cooldownImage.transform.localScale;

            cooldownText.transform.localScale = textDefaultScale * EFFECT_SCALE_MODIFIER;
            cooldownImage.transform.localScale = imageDefaultScale * EFFECT_SCALE_MODIFIER;
            cooldownText.DOFade(0, 0);

            cooldownImage.material.SetFloat("_FillPercent", 0);
        }

        private void Update()
        {
            if (update)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                    timer = 0;
                UpdateUI();
            }
        }

        private void StartCooldown()
        {
            timer = RewindManager.REWIND_COOLDOWN;
            cooldownText.transform.DOScale(textDefaultScale, 1.5f).SetEase(Ease.OutCubic);
            cooldownText.DOFade(1, 1.5f).SetEase(Ease.OutCubic);
            cooldownImage.transform.DOScale(imageDefaultScale, 1.5f).SetEase(Ease.OutCubic);
            cooldownImage.material.SetFloat("_FillPercent", 1);
            cooldownImage.material.DOFloat(0, "_FillPercent", RewindManager.REWIND_COOLDOWN).SetEase(Ease.Linear).onComplete += StopCooldown;
            update = true;
        }

        private void StopCooldown()
        {
            cooldownText.transform.DOScale(textDefaultScale * EFFECT_SCALE_MODIFIER, 1.5f).SetEase(Ease.OutCubic);
            cooldownText.DOFade(0, 1.5f).SetEase(Ease.OutCubic);
            cooldownImage.transform.DOScale(imageDefaultScale * EFFECT_SCALE_MODIFIER, 1.5f).SetEase(Ease.OutCubic);
            cooldownImage.material.SetFloat("_FillPercent", 0);
            update = false;
        }

        private void UpdateUI()
        {
            cooldownText.text = timer.ToString("f0");
        }
    }
}