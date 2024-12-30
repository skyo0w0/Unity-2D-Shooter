using System;
using UIComponents;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DeathResponce
{
    public class PlayerIDeathResponseComponent : MonoBehaviour , IDeathResponse
    {
        private EnemyScoreComponent _enemyScoreComponent;
        
        [Inject]
        public void Construct(EnemyScoreComponent enemyScoreComponent)
        {
            _enemyScoreComponent = enemyScoreComponent;
        }
        
        public void OnDeath()
        {
            _enemyScoreComponent.AddScore(10);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}