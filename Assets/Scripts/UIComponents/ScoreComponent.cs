using System;
using Unity.VisualScripting;
using UnityEngine;

namespace UIComponents
{
    public abstract class ScoreComponent : MonoBehaviour
    {
        [SerializeField] private int _score = 0;

        public event Action<int> OnScoreChanged;

        // Абстрактное свойство для ключа
        protected abstract string ScoreKey { get; }

        private void Start()
        {
            _score = PlayerPrefs.GetInt(ScoreKey, 0);
        }

        private void OnApplicationQuit()
        {
            ResetScore();
        }

        protected virtual void SaveScore()
        {
            PlayerPrefs.SetInt(ScoreKey, _score);
            PlayerPrefs.Save();
            OnScoreChanged?.Invoke(_score);
        }

        public virtual void AddScore(int amount)
        {
            _score += amount;
            SaveScore();
        }

        public virtual void ResetScore()
        {
            _score = 0;
            SaveScore();
        }

        public virtual int GetScore()
        {
            return _score;
        }
    }
}
