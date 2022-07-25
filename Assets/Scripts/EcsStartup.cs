using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity;
//using Leopotam.EcsLite.UnityEditor;
using UnityEngine;

namespace Client
{
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] EcsUguiEmitter _uguiEmitter;
        [SerializeField] private TowerStorage _towerStorage;
        [SerializeField] private InterfaceStorage _interfaceStorage;
        [SerializeField] private PlayerStorage _playerStorage;
        [SerializeField] private DefenseTowerStorage _defenseTowerStorage;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private int _towerCount;
        private WaveStorage _waveStorage;
        EcsSystems _systems;
        EcsSystems _delHereSystems;
        EcsSystems _systemsFixed;
        EcsWorld _world = null;
        GameState _gameState = null;

        void Start ()
        {
            _waveStorage = gameObject.GetComponent<WaveStorage>();
            _world = new EcsWorld();
            _gameState = new GameState(_world, _towerStorage, _interfaceStorage, 
            _playerStorage, _defenseTowerStorage, _towerCount, _waveStorage, _enemyConfig);
            _systems = new EcsSystems (_world, _gameState);
            _systemsFixed = new EcsSystems(_world, _gameState);
            _delHereSystems = new EcsSystems(_world, _gameState);
            
            _systems
                .Add(new CheckWinSystem())
                .Add(new WinSystem())
                //.Add(new CheckLoseSystem())
                .Add(new LoseSystem())
                
                .Add(new InitMainTower())

                .Add(new StartWaveSystem())
                .Add(new StartCurrentWave())

                .Add(new InitCamera())
                .Add(new PlayerInitSystem())
                .Add(new OreInitSystem())
                .Add(new InitInterfaceSystem())
                //.Add(new RadiusInitSystem())

                .Add(new HPRegenerationSystem())
                .Add(new CooldownSystem())

                .Add(new OnOffTowerAttack())
                
                .Add(new DefendersFallbackSystem())

                .Add(new UnitMovingSystem())
                .Add(new UnitStandingSystem())
                .Add(new DistanceToTargetSystem())
                .Add(new OnOffUnitAttack())

                .Add(new ProjectileFlyingSystem())

                .Add(new TargetingSystem())
                .Add(new PlayerTargetingSystem())

                .Add(new TowerLookingSystem())
                .Add(new TowerShotSystem())

                .Add(new ShipMovingSystem())
                .Add(new ShipArrivalSystem())

                .Add(new WaveSwitcher())
                .Add(new EncounterSwitcher())

                //.Add(new CheckCircleMiningSystem())
                .Add(new UserInputSystem())
                .Add(new AddCoinSystem())
                .Add(new ItemMoveToBagSystem())
                .Add(new OreMiningSystem())
                .Add(new OreMoveSystem())
                .Add(new StoneMiningSystem())
                .Add(new CameraFollowSystem())

                .Add(new UpgradeSystems())
                .Add(new CreateNextTowerSystem())
                .Add(new CreateNewPlayer())
                .Add(new CreateDefenders())
                .Add(new LevelPopupSystem())

                .Add(new DamagingEventSystem())
                .Add(new TargetingEventSystem())
                .Add(new DamagePopupSystem())

                .Add(new ReloadSystem())
                .Add(new DieSystem())
                .Add(new UserDropSystem())
                .Add(new DropResourcesSystem())
                .Add(new DroppedGoldSystem())
                //.Add(new RespawnDefender())
                .Add(new RespawnEventSystem())
                //.Add(new OreRespawnSystem())
                .Add(new UpgradeCanvasSystem())
                .Add(new CanvasShipPointerSystem())
                .Add(new CanvasPointerSystem())
                .Add(new CoinPickupSystem())
                .Add(new NewTowersCircleSystem())
                .Add(new SavesSystem())

                .Add(new VibrationEventSystem())

                .AddWorld(new EcsWorld(), Idents.Worlds.Events)
                .DelHere<ShipArrivalEvent>()
                .DelHere<ActivateWaveShipsEvent>()
                .DelHere<DamagingEvent>()
                .DelHere<TargetingEvent>()
                .DelHere<CreateNextTowerEvent>()
                ;
            _systemsFixed
                .Add(new UserMoveSystem())
                .Add(new UnitLookingSystem())
                .Add(new ActivateContextToolEventSystem())
                ;

#if UNITY_EDITOR
            _systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
            _systemsFixed.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
                //_systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(Idents.Worlds.Events))
                //.Add(new EcsWorldDebugSystem(Idents.Worlds.Events))
#endif         
            _systems
                .Inject()
                .InjectUgui(_uguiEmitter, Idents.Worlds.Events)
                .Init();
            _systemsFixed.Inject()
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
        private void FixedUpdate()
        {
            _systemsFixed?.Run();
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