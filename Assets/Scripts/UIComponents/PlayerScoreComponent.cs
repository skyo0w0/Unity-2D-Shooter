using UnityEngine;

namespace UIComponents
{
    public class PlayerScoreComponent : ScoreComponent
    {
        protected override string ScoreKey => "PlayerScore"; // Уникальный ключ для игрока

        public override void ResetScore()
        {
            base.ResetScore();
            Debug.Log("Player score has been reset.");
        }

        public override void AddScore(int amount)
        {
            base.AddScore(amount);
            Debug.Log($"Player score updated: {GetScore()}");
        }
    }
}