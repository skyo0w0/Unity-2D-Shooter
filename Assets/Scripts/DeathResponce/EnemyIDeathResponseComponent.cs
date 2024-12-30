using System;
using UIComponents;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DeathResponce
{
    public class EnemyIDeathResponseComponent : MonoBehaviour , IDeathResponse
    {
        private PlayerScoreComponent _playerScoreComponent;
        
        [Inject]
        public void Construct(PlayerScoreComponent playerScoreComponent)
        {
           _playerScoreComponent = playerScoreComponent;
        }
        
        public void OnDeath()
        {
            _playerScoreComponent.AddScore(10);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}