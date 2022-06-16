using System;
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
        readonly EcsPoolInject<TowerTag> _tPool = default;
        private float Angle = 0;

        public void Init (EcsSystems systems)
        {
            var entity = _world.Value.NewEntity();
            string towerID = "1tower";
            _state.Value.CurrentTowerID = towerID;
            
            _towerPool.Value.Add(entity);
            ref var radiusComp = ref _radiusPool.Value.Add(entity);
            radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(towerID);

            ref var healthComp = ref _healthPool.Value.Add(entity);
            healthComp.Health = _state.Value.TowerStorage.GetHealthByID(towerID);

            var mainTower = GameObject.Instantiate(_state.Value.TowerStorage.GetTowerPrefabByID(towerID), Vector3.zero, Quaternion.identity);

            ref var viewComponent = ref _viewPool.Value.Add(entity);
            viewComponent.GameObject = mainTower;

            for (int i = 0; i < _state.Value.TowersEntity.Length;i++)
            {
                if(i == 0) _state.Value.TowersEntity[i] = entity;
                else
                {
                    int towerEntity = _world.Value.NewEntity();
                    _state.Value.TowersEntity[i] = towerEntity;
                    _viewPool.Value.Add(towerEntity);
                    _healthPool.Value.Add(towerEntity);
                    _radiusPool.Value.Add(towerEntity);
                    ref var tComp = ref _tPool.Value.Add(towerEntity);
                    
                    var x = Mathf.Cos(Angle * Mathf.Deg2Rad) * radiusComp.Radius;
                    
                    var z = Mathf.Sin(Angle * Mathf.Deg2Rad) * radiusComp.Radius;
                    tComp.Position = new Vector3(x, 0, z);
                    
                    Angle += 360 / (_state.Value.TowerCount - 1);
                }
            }
        }
    }
}