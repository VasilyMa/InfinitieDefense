using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity;
using Leopotam.EcsLite.UnityEditor;
using UnityEngine;

namespace Client
{
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] EcsUguiEmitter _uguiEmitter;
        EcsSystems _systems;
        EcsWorld _world = null;
        GameState _gameState = null;

        void Start ()
        {
            _world = new EcsWorld();
            _gameState = new GameState(_world);
            _systems = new EcsSystems (_world, _gameState);
            _systems
                //.Add(new PlayerInitSystem())
                //.Add(new UserInputSystem())
                .Add(new InitEnemyUnits())
                .Add(new InitEnemyShips())
                .Add(new InitMainTower())
                .Add(new EnemyTargetingSystem())
                .Add(new LookingSystem())
                .Add(new EnemyMovingSystem())
                .Add(new DistanceToTargetSystem())
                .Add(new JoinToFightSystem())

                //.AddWorld(new EcsWorld(), Idents.Worlds.Events)

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                //.Add(new EcsWorldDebugSystem(Idents.Worlds.Events))
#endif
                .Inject()
                //.InjectUgui(_uguiEmitter, Idents.Worlds.Events)
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