using DeathResponce;
using Player;
using UIComponents;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject _playerPrefab; 
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private GameObject _enemyPrefab; 
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private GameObject canvasPrefab;
    public override void InstallBindings()
    {
        Container.Bind<InputHandler>()
            .FromNewComponentOnNewGameObject()
            .AsSingle()
            .NonLazy();
        
        var playerInstance = Container.InstantiatePrefab(
            _playerPrefab, 
            _playerSpawnPoint.position, 
            Quaternion.identity, 
            null
        );
        
        
        Container.Bind<PlayerMovementComponent>()
            .FromInstance(playerInstance.GetComponent<PlayerMovementComponent>()).AsSingle().NonLazy();
            
        
        var playerScoreComponent = playerInstance.GetComponent<PlayerScoreComponent>();
        
        Container.Bind<PlayerScoreComponent>().FromInstance(playerScoreComponent).AsSingle().NonLazy();

        var enemyInstance = Container.InstantiatePrefab(
            _enemyPrefab,
            _enemySpawnPoint.position,
            Quaternion.identity,
            null
        );
        
        var enemyScoreComponent = enemyInstance.GetComponent<EnemyScoreComponent>();
        
        Container.Bind<EnemyScoreComponent>().FromInstance(enemyScoreComponent).AsSingle().NonLazy();
        
        Container.Bind<IDeathResponse>()
            .To<PlayerIDeathResponseComponent>()
            .FromNewComponentOn(playerInstance)
            .AsSingle().NonLazy();

        Container.Bind<IDeathResponse>()
            .To<EnemyIDeathResponseComponent>()
            .FromNewComponentOn(enemyInstance)
            .AsSingle().NonLazy();

        var canvasGameObject = Container.InstantiatePrefab(canvasPrefab);
        
    }
}