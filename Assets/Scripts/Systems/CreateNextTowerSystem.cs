using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CreateNextTowerSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<CreateNextTowerEvent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<RadiusComponent> _radiusPool = default;
        readonly EcsPoolInject<TowerTag> _towerPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<TargetWeightComponent> _targetWeightPool = default;
        readonly EcsPoolInject<CreateDefenderEvent> _defenderPool = default;
        readonly EcsFilterInject<Inc<CanvasUpgradeComponent, UpgradeCanvasEvent>> _canvasFilter = default;
        readonly EcsPoolInject<CanvasUpgradeComponent> _upgradePool = default;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);

                int towerIndex = filterComp.TowerIndex;
                ref var radiusComp = ref _radiusPool.Value.Get(entity);
                ref var viewComp = ref _viewPool.Value.Get(entity);
                if(viewComp.GameObject != null) GameObject.Destroy(viewComp.GameObject);

                viewComp.UpgradeParticleSystem.Play();
                
                if (towerIndex == 0)
                {
                    _state.Value.DefenseTowers[towerIndex] = _state.Value.TowerStorage.GetNextIDByID(_state.Value.DefenseTowers[towerIndex]);
                    viewComp.GameObject = GameObject.Instantiate(_state.Value.TowerStorage.GetTowerPrefabByID(_state.Value.DefenseTowers[towerIndex]), Vector3.zero, Quaternion.identity);
                    radiusComp.RadiusTransform = GameObject.Instantiate(_state.Value.InterfaceStorage.RadiusPrefab, viewComp.GameObject.transform).GetComponent<Transform>();
                    radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(_state.Value.DefenseTowers[towerIndex]);


                    _defenderPool.Value.Add(_world.Value.NewEntity());

                    viewComp.Healthbar = viewComp.GameObject.GetComponent<HealthbarMB>();
                    viewComp.Healthbar.SetMaxHealth(_state.Value.TowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.SetHealth(_state.Value.TowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());
                }
                else
                {
                    ref var towerComp = ref _towerPool.Value.Get(entity);
                    if (!_targetablePool.Value.Has(entity))
                    {
                        _targetablePool.Value.Add(entity);
                    }

                    if (!_damagePool.Value.Has(entity))
                    {
                        _damagePool.Value.Add(entity);
                    }

                    if (!_targetWeightPool.Value.Has(entity))
                    {
                        _targetWeightPool.Value.Add(entity);
                    }

                    ref var targetableComponent = ref _targetablePool.Value.Get(entity);
                    ref var damageComponent = ref _damagePool.Value.Get(entity);
                    ref var targetWeightComponent = ref _targetWeightPool.Value.Get(entity);

                    _state.Value.DefenseTowers[towerIndex] = _state.Value.DefenseTowerStorage.GetNextIDByID(_state.Value.DefenseTowers[towerIndex]);
                    viewComp.GameObject = GameObject.Instantiate(_state.Value.DefenseTowerStorage.GetTowerPrefabByID(_state.Value.DefenseTowers[towerIndex]), towerComp.Position, Quaternion.identity);
                    radiusComp.Radius = _state.Value.DefenseTowerStorage.GetRadiusByID(_state.Value.DefenseTowers[towerIndex]);
                    radiusComp.RadiusTransform = GameObject.Instantiate(_state.Value.InterfaceStorage.RadiusPrefab, viewComp.GameObject.transform).GetComponent<Transform>();
                    viewComp.Healthbar = viewComp.GameObject.GetComponent<HealthbarMB>();
                    viewComp.Healthbar.SetMaxHealth(_state.Value.TowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.SetHealth(_state.Value.TowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());
                    viewComp.SphereCollider = viewComp.GameObject.GetComponent<SphereCollider>();
                    viewComp.SphereCollider.radius = radiusComp.Radius;

                    damageComponent.Value = 1; //ispravit'

                    viewComp.EcsInfoMB = viewComp.GameObject.GetComponent<EcsInfoMB>();
                    viewComp.EcsInfoMB.Init(_world);
                    viewComp.EcsInfoMB.SetEntity(entity);

                    viewComp.TowerAttackMB = viewComp.GameObject.GetComponent<TowerAttackMB>();
                    viewComp.TowerAttackMB.Init(_world);

                    targetWeightComponent.Value = 5;
                }

                //radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(_state.Value.CurrentTowerID);
                radiusComp.RadiusTransform.localScale = new Vector3(radiusComp.Radius * 2, radiusComp.Radius * 2, 1);

                _filter.Pools.Inc1.Del(entity);

            }
        }
    }
}