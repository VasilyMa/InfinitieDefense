using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class PlayerInitSystem : IEcsInitSystem {

        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<CooldownComponent> _cooldownMining = default;
        readonly EcsPoolInject<ReloadComponent> _reloadPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<TargetWeightComponent> _targetWeightPool = default;
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        public void Init (EcsSystems systems) 
        {
            
            var playerEntity = _playerPool.Value.GetWorld().NewEntity();
            _state.Value.EntityPlayer = playerEntity;
            _world.Value.GetPool<UnitTag>().Add(playerEntity);
            ref var player = ref _playerPool.Value.Add (playerEntity);
            ref var viewComponent = ref _viewPool.Value.Add(playerEntity);
            ref var healthComponent = ref _world.Value.GetPool<HealthComponent>().Add(playerEntity);
            var PlayerGo = GameObject.Instantiate(_state.Value.PlayerStorage.GetPlayerByID(_state.Value.CurrentPlayerID), new Vector3(0,2,-10), Quaternion.identity);

            player.Transform = PlayerGo.transform;
            player.playerMB = PlayerGo.GetComponent<PlayerMB>();
            player.rigidbody = PlayerGo.GetComponent<Rigidbody>();
            player.MoveSpeed = 15f;
            player.RotateSpeed = 1f;
            player.damage = _state.Value.PlayerStorage.GetDamageByID(_state.Value.CurrentPlayerID);
            player.health = _state.Value.PlayerStorage.GetHealthByID(_state.Value.CurrentPlayerID);
            player.ResHolderTransform = PlayerGo.transform.GetChild(2).transform;
            player.animator = PlayerGo.GetComponent<Animator>();
            player.playerMB.Init(systems.GetWorld(), systems.GetShared<GameState>());

            viewComponent.GameObject = PlayerGo;
            viewComponent.Animator = PlayerGo.GetComponent<Animator>();
            viewComponent.Healthbar = PlayerGo.GetComponent<HealthbarMB>();
            viewComponent.Healthbar.SetMaxHealth(player.health);
            viewComponent.Healthbar.SetHealth(player.health);
            viewComponent.Healthbar.ToggleSwitcher();
            viewComponent.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());
            viewComponent.EcsInfoMB = PlayerGo.GetComponent<EcsInfoMB>();
            viewComponent.EcsInfoMB.SetEntity(playerEntity);
            viewComponent.PlayerAttackMB = PlayerGo.GetComponent<PlayerAttackMB>();
            viewComponent.PlayerAttackMB.Init(_world);
            viewComponent.SkinnedMeshRenderer = PlayerGo.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
            viewComponent.UpgradeParticleSystem = PlayerGo.transform.GetChild(4).transform.GetChild(0).GetComponent<ParticleSystem>();

            ref var targetWeightComponent = ref _targetWeightPool.Value.Add(playerEntity);
            targetWeightComponent.Value = 10;

            healthComponent.MaxValue = 100;
            healthComponent.CurrentValue = healthComponent.MaxValue;

            var colliderChecker = PlayerGo.GetComponent<ColliderChecker>();
            colliderChecker.Init(systems.GetWorld(), systems.GetShared<GameState>());

            _cooldownMining.Value.Add(_state.Value.EntityPlayer);
            ref var cooldown = ref _cooldownMining.Value.Get(_state.Value.EntityPlayer);
            cooldown.maxValue = 2f;
            cooldown.currentValue = 0;
            _reloadPool.Value.Add(_state.Value.EntityPlayer);

            
            
            
            
        }
    }
}