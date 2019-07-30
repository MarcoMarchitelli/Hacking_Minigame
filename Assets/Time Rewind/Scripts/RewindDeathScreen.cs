using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Rewind
{
    public class RewindDeathScreen : MonoBehaviour
    {
        public Color screenColor;
        public Color textColor;
        public float fadeInTime;

        [Header("References")]
        public Image deathImage;
        public MenuUI menuUI;
        public TextMeshProUGUI[] deathTexts;
        public TextMeshProUGUI currentScoreText;
        public TextMeshProUGUI highScoreText;

        public System.Action OnDeathScreenEnd;
        Sequence deathScreenSequence;

        private void Awake()
        {
            RewindPlayer player = FindObjectOfType<RewindPlayer>();
            if (player)
            {
                player.OnDeath += Play;
                deathScreenSequence = DOTween.Sequence();
            }
        }

        void Play()
        {          
            PlayerPrefs.SetInt("CurrentScore", EnemySpawner.ScoreCount);

            if (EnemySpawner.ScoreCount > PlayerPrefs.GetInt("HighScore"))
                PlayerPrefs.SetInt("HighScore", EnemySpawner.ScoreCount);

            menuUI.FadeButtons(true, fadeInTime, () => menuUI.enabled = true);
            menuUI.SelectFirst();
            deathScreenSequence.Append(deathImage.DOColor(screenColor, fadeInTime));

            foreach (TextMeshProUGUI text in deathTexts)
            {
                deathScreenSequence.Join(text.DOFade(1, fadeInTime));
            }

            currentScoreText.text = PlayerPrefs.GetInt("CurrentScore", 0).ToString();
            highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

            ////RESET ___________________________________
            //PlayerPrefs.SetInt("CurrentScore", 0);
            //PlayerPrefs.SetInt("HighScore", 0);
            ////_________________________________________

            deathScreenSequence.Play();
        }
    } 
}