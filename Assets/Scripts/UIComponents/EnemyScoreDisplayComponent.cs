using Zenject;
using UnityEngine;

namespace UIComponents
{
    public class EnemyScoreDisplayComponent :  ScoreDisplayComponent
    {
        [Inject] private EnemyScoreComponent _enemyScoreComponent;

        protected override ScoreComponent ScoreComponent => _enemyScoreComponent;
    }
}