using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;

namespace Client
{
    sealed class PlayerInitSystem : IEcsInitSystem
    {

        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<CooldownComponent> _cooldownMining = default;
        readonly EcsPoolInject<ReloadComponent> _reloadPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<TargetWeightComponent> _targetWeightPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<CanvasPointerComponent> _pointerPool = default;
        readonly EcsPoolInject<HPRegeneration> _regenerationPool = default;
        readonly EcsPoolInject<UnitTag> _unitPool = default;
        public void Init (EcsSystems systems) 
        {
            var playerEntity = _playerPool.Value.GetWorld().NewEntity();

            _state.Value.EntityPlayer = playerEntity;

            _unitPool.Value.Add(playerEntity);

            ref var player = ref _playerPool.Value.Add (playerEntity);
            ref var viewComponent = ref _viewPool.Value.Add(playerEntity);
            ref var healthComponent = ref _healthPool.Value.Add(playerEntity);
            ref var pointerComponent = ref _pointerPool.Value.Add(playerEntity);
            ref var targetWeightComponent = ref _targetWeightPool.Value.Add(playerEntity);
            ref var damageComponent = ref _damagePool.Value.Add(playerEntity);
            ref var targetableComponent = ref _targetablePool.Value.Add(playerEntity);
            ref var regenerationComponent = ref _regenerationPool.Value.Add(playerEntity);
            var PlayerGo = GameObject.Instantiate(_state.Value.PlayerStorage.GetPlayerByID(_state.Value.CurrentPlayerID), new Vector3(0,2,-10), Quaternion.identity);

            player.Transform = PlayerGo.transform;
            player.playerMB = PlayerGo.GetComponent<PlayerMB>();
            player.rigidbody = PlayerGo.GetComponent<Rigidbody>();
            player.MoveSpeed = _state.Value.PlayerStorage.GetSpeedByID(_state.Value.CurrentPlayerID);
            player.RotateSpeed = 10f;
            player.damage = _state.Value.PlayerStorage.GetDamageByID(_state.Value.CurrentPlayerID);
            player.health = _state.Value.PlayerStorage.GetHealthByID(_state.Value.CurrentPlayerID);
            player.ResHolderTransform = PlayerGo.transform.GetChild(2).transform;
            player.animator = PlayerGo.GetComponent<Animator>();
            player.playerMB.Init(systems.GetWorld(), systems.GetShared<GameState>());

            viewComponent.GameObject = PlayerGo;
            viewComponent.Rigidbody = PlayerGo.GetComponent<Rigidbody>();
            viewComponent.Transform = PlayerGo.transform;
            viewComponent.WeaponHolder = GameObject.Find("WeaponHolder").transform;
            viewComponent.Animator = PlayerGo.GetComponent<Animator>();
            viewComponent.Healthbar = PlayerGo.GetComponent<HealthbarMB>();
            viewComponent.Level = PlayerGo.GetComponent<LevelMB>();
            viewComponent.Healthbar.SetMaxHealth(_state.Value.PlayerStorage.GetHealthByID(_state.Value.CurrentPlayerID));
            viewComponent.Healthbar.SetHealth(_state.Value.PlayerStorage.GetHealthByID(_state.Value.CurrentPlayerID));
            viewComponent.Healthbar.ToggleSwitcher();
            viewComponent.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());
            viewComponent.Level.UpdateLevel(_state.Value.PlayerLevel);
            viewComponent.Level.Init(systems.GetWorld(), systems.GetShared<GameState>());
            viewComponent.EcsInfoMB = PlayerGo.GetComponent<EcsInfoMB>();
            viewComponent.EcsInfoMB.Init(_world);
            viewComponent.EcsInfoMB.SetEntity(playerEntity);
            viewComponent.SkinnedMeshRenderer = PlayerGo.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
            viewComponent.UpgradeParticleSystem = PlayerGo.transform.GetChild(4).transform.GetChild(0).GetComponent<ParticleSystem>();
            viewComponent.HitParticleSystem = PlayerGo.transform.GetChild(4).transform.GetChild(1).GetComponent<ParticleSystem>();
            viewComponent.DropItemParticleSystem = PlayerGo.transform.GetChild(4).transform.GetChild(2).GetComponent<ParticleSystem>();
            viewComponent.Regeneration = PlayerGo.transform.GetChild(4).transform.GetChild(3).GetComponent<ParticleSystem>();
            //viewComponent.DamagePopup = GameObject.Find("CanvasDamagePopup");
            //viewComponent.DamagePopup.SetActive(false);
            pointerComponent.player = PlayerGo;

            targetWeightComponent.Value = 5;

            damageComponent.Value = _state.Value.PlayerStorage.GetDamageByID(_state.Value.CurrentPlayerID);

            healthComponent.MaxValue = _state.Value.PlayerStorage.GetHealthByID(_state.Value.CurrentPlayerID);
            healthComponent.CurrentValue = healthComponent.MaxValue;

            targetableComponent.AllEntityInDamageZone = new List<int>();
            targetableComponent.AllEntityInDetectedZone = new List<int>();
            targetableComponent.TargetEntity = -1;
            targetableComponent.TargetObject = null;

            regenerationComponent.MaxCooldown = 2;
            regenerationComponent.CurrentCooldown = regenerationComponent.MaxCooldown;
            regenerationComponent.Value = 10;
            regenerationComponent.OldHPValue = healthComponent.CurrentValue;

            var colliderChecker = PlayerGo.GetComponent<ColliderChecker>();
            colliderChecker.Init(systems.GetWorld(), systems.GetShared<GameState>());

            _cooldownMining.Value.Add(_state.Value.EntityPlayer);
            ref var cooldown = ref _cooldownMining.Value.Get(_state.Value.EntityPlayer);
            cooldown.maxValue = 2f;
            cooldown.currentValue = 0;
            _reloadPool.Value.Add(_state.Value.EntityPlayer);

            //todo заполнить ресхолдер монетками и камнями
            for (int i = 0; i < _state.Value.RockCount;i++)
            {
                var rockTransform = GameObject.Instantiate(_state.Value.InterfaceStorage.RockPrefab, new Vector3(0, i * 0.6f, 0), Quaternion.identity,player.ResHolderTransform).GetComponent<Transform>();
                rockTransform.localPosition = new Vector3(0, i * 0.6f, 0);
                _state.Value.StoneTransformList.Add(rockTransform);
            }
            for (int i = 0; i < _state.Value.CoinCount;i++)
            {
                var coinTransform = GameObject.Instantiate(_state.Value.InterfaceStorage.GoldPrefab, new Vector3(0, _state.Value.RockCount * 0.6f + i * 0.3f, 0), Quaternion.identity, player.ResHolderTransform).GetComponent<Transform>();
                coinTransform.localPosition = new Vector3(0, _state.Value.RockCount * 0.6f + i * 0.3f, 0);
                _state.Value.CoinTransformList.Add(coinTransform);
            }
        }
    }
}