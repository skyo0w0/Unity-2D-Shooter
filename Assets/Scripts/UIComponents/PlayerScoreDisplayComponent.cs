using Zenject;

namespace UIComponents
{
    public class PlayerScoreDisplayComponent : ScoreDisplayComponent
    {
        [Inject] private PlayerScoreComponent _playerScoreComponent;

        protected override ScoreComponent ScoreComponent => _playerScoreComponent;

    }
}