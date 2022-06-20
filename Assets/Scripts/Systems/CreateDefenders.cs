using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CreateDefenders : IEcsRunSystem {

        readonly EcsWorldInject _world = default;

        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CreateDefenderEvent>> _filter = default;
        readonly EcsPoolInject<MainTowerTag> _mainTowerPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<TargetWeightComponent> _targetWeightPool = default;
        readonly EcsPoolInject<Movable> _movablePool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<UnitTag> _unitPool = default;

        //todo components

        public void Run (EcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var mainTowerComp = ref _mainTowerPool.Value.Get(_state.Value.TowersEntity[0]);
                int count = _state.Value.TowerStorage.GetDefenderCountByID(_state.Value.DefenseTowers[0]);

                for (int i = 0; i < count;i++)
                {
                    if(_state.Value.DefendersGOs[i] == null)
                    {
                        int defenderEntity = _state.Value.DefendersEntity[i];

                        _state.Value.DefendersGOs[i] = GameObject.Instantiate(_state.Value.TowerStorage.DefenderPrefab, mainTowerComp.DefendersPositions[i], Quaternion.identity);
                        //todo заполнить энтити дефендера


                        _unitPool.Value.Add(defenderEntity);

                        ref var viewComponent = ref _viewPool.Value.Add(defenderEntity);
                        ref var healthComponent = ref _healthPool.Value.Add(defenderEntity);
                        ref var targetWeightComponent = ref _targetWeightPool.Value.Add(defenderEntity);
                        ref var movableComponent = ref _movablePool.Value.Add(defenderEntity);
                        ref var damageComponent = ref _damagePool.Value.Add(defenderEntity);

                        damageComponent.Value = 10;

                        movableComponent.Speed = 10;

                        targetWeightComponent.Value = 10;

                        viewComponent.GameObject = _state.Value.DefendersGOs[i];
                        viewComponent.Transform = viewComponent.GameObject.transform;
                        viewComponent.Rigidbody = viewComponent.GameObject.GetComponent<Rigidbody>();

                        viewComponent.EcsInfoMB = viewComponent.GameObject.GetComponent<EcsInfoMB>();
                        viewComponent.EcsInfoMB.Init(_world);
                        viewComponent.EcsInfoMB.SetEntity(defenderEntity);

                        viewComponent.Healthbar = viewComponent.GameObject.GetComponent<HealthbarMB>();
                        viewComponent.Healthbar.SetMaxHealth(healthComponent.MaxValue);
                        viewComponent.Healthbar.SetHealth(healthComponent.MaxValue);
                        viewComponent.Healthbar.ToggleSwitcher();
                        viewComponent.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());

                        viewComponent.AttackMB = viewComponent.GameObject.GetComponent<AttackMB>();
                        viewComponent.AttackMB.Init(_world);

                        viewComponent.Outline = viewComponent.GameObject.GetComponent<Outline>();

                        healthComponent.MaxValue = 1000;
                        healthComponent.CurrentValue = healthComponent.MaxValue;
                    }
                }

                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}