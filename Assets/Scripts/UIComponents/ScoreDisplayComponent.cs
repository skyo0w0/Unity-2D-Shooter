using Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace UIComponents
{
    public abstract class ScoreDisplayComponent : MonoBehaviour
    {
        protected virtual TMP_Text _scoreText { get; set; }
        protected virtual ScoreComponent ScoreComponent { get; }

        protected virtual void Start()
        {
            _scoreText = GetComponent<TMP_Text>();
            _scoreText.text = ScoreComponent.GetScore().ToString();
        }

        protected virtual void OnEnable()
        {
            if (ScoreComponent != null)
            {
                ScoreComponent.OnScoreChanged += UpdateScoreText;
            }
        }

        protected virtual void OnDisable()
        {
            if (ScoreComponent != null)
            {
                ScoreComponent.OnScoreChanged -= UpdateScoreText;
            }
        }

        protected virtual void UpdateScoreText(int newScore)
        {
            _scoreText.text = $"{newScore}";
        }
    }
}