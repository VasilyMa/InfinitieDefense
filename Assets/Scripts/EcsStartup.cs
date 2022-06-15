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
                .Add(new InitMainTower())
                .Add(new InitEnemyUnits())
                .Add(new InitEnemyShips())
                .Add(new InitCamera())
                .Add(new PlayerInitSystem())
                .Add(new OreInitSystem())
                .Add(new InitInterfaceSystem())
                .Add(new RadiusInitSystem())
                .Add(new EnemyTargetingSystem())
                .Add(new LookingSystem())
                .Add(new EnemyMovingSystem())
                .Add(new DistanceToTargetSystem())
                .Add(new JoinToFightSystem())
                .Add(new ShipArrivalSystem())

                //.AddWorld(new EcsWorld(), Idents.Worlds.Events)
                .Add(new StoneMiningSystem())
                .Add(new UserInputSystem())
                .Add(new AddCoinSystem())
                .Add(new RaycastUserSystem())
                .Add(new OreMiningSystem())
                .Add(new ReloadMiningSystem())
                .Add(new CameraFollowSystem())
                .Add(new UpgradeSystems())
                .Add(new CreateNextTowerSystem())

                .AddWorld(new EcsWorld(), Idents.Worlds.Events)
                .DelHere<ShipArrivalEvent>()

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                .Add(new EcsWorldDebugSystem(Idents.Worlds.Events))
#endif
                .Inject()
                .InjectUgui(_uguiEmitter, Idents.Worlds.Events)
                .Init();
            _delHereSystems.Inject();
            _delHereSystems.Init();

            /*_delHereSystems
                .DelHere<StoneMiningEvent>()
                .DelHere<AddCoinEvent>()
                ;*/
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