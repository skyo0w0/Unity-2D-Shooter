using UnityEngine;

namespace UIComponents
{
    public class EnemyScoreComponent : ScoreComponent
    {
        protected override string ScoreKey => "EnemyScore"; // Уникальный ключ для врага

        public override void ResetScore()
        {
            base.ResetScore();
            Debug.Log("Enemy score has been reset.");
        }

        public override void AddScore(int amount)
        {
            base.AddScore(amount);
            Debug.Log($"Enemy score updated: {GetScore()}");
        }
    }
}