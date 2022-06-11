using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity;
using Leopotam.EcsLite.UnityEditor;
using UnityEngine;

namespace Client
{
    sealed class EcsStartup : MonoBehaviour
    {
        EcsSystems _systems;
        EcsWorld _world = null;
        GameState _gameState = null;

        void Start ()
        {
            _world = new EcsWorld();
            _gameState = new GameState(_world);
            _systems = new EcsSystems (_world, _gameState);
            _systems
                .Add (new InitEnemyUnits())
                .Add (new InitMainTower())
                .Add (new EnemyTargetingSystem())
                .Add (new EnemyMovingSystem())

#if UNITY_EDITOR
                .Add (new EcsWorldDebugSystem())
#endif
                .Inject()
                .Init();
        }

        void Update()
        {
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems.GetWorld().Destroy();
                _systems = null;
            }
        }
    }
}