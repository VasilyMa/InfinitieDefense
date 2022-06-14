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
        [SerializeField] private TowerStorage _towerStorage;
        [SerializeField] private InterfaceStorage _interfaceStorage;
        [SerializeField] private PlayerStorage _playerStorage;
        EcsSystems _systems;
        EcsSystems _delHereSystems;
        EcsWorld _world = null;
        GameState _gameState = null;

        void Start ()
        {
            _world = new EcsWorld();
            _gameState = new GameState(_world, _towerStorage, _interfaceStorage, _playerStorage);
            _systems = new EcsSystems (_world, _gameState);
            _delHereSystems = new EcsSystems(_world, _gameState);

            _systems
                .Add(new PlayerInitSystem())
                .Add(new InitInterfaceSystem())
                .Add(new InitEnemyUnits())
                .Add(new InitMainTower())
                .Add(new RadiusInitSystem())
                .Add(new EnemyTargetingSystem())
                .Add(new EnemyMovingSystem())
                .Add(new StoneMiningSystem())
                .Add(new UserInputSystem())
                .Add(new AddCoinSystem())

                .AddWorld(new EcsWorld(), Idents.Worlds.Events)

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                .Add(new EcsWorldDebugSystem(Idents.Worlds.Events))
#endif
                .Inject()
                .InjectUgui(_uguiEmitter, Idents.Worlds.Events)
                .Init();
            _delHereSystems.Inject();
            _delHereSystems.Init();

            _delHereSystems
                .DelHere<StoneMiningEvent>()
                .DelHere<AddCoinEvent>()
                ;
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