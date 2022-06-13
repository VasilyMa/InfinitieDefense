using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitMainTower : IEcsInitSystem
    {
        readonly EcsSharedInject<GameState> _state;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<MainTowerTag> _towerPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<RadiusComponent> _radiusPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;

        public void Init (EcsSystems systems)
        {
            var entity = _world.Value.NewEntity();
            _state.Value.EntityMainTower = entity;
            string towerID = "1tower";

            _towerPool.Value.Add(entity);
            ref var radiusComp = ref _radiusPool.Value.Add(entity);
            radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(towerID);

            ref var healthComp = ref _healthPool.Value.Add(entity);
            healthComp.Health = _state.Value.TowerStorage.GetHealthByID(towerID);

            var mainTower = GameObject.Instantiate(_state.Value.TowerStorage.GetTowerPrefabByID(towerID), new Vector3(0,0.5f,0), Quaternion.identity);

            ref var viewComponent = ref _viewPool.Value.Add(entity);
            viewComponent.GameObject = mainTower;
            
        }
    }
}