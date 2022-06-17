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
        readonly EcsPoolInject<DefenderComponent> _defenderPool = default;
        private float Angle = 0;

        public void Init (EcsSystems systems)
        {
            var entity = _world.Value.NewEntity();
            string towerID = "1tower";
            _state.Value.CurrentTowerID = towerID;
            
            ref var towerComp = ref _towerPool.Value.Add(entity);
            towerComp.DefendersPositions = new Vector3[10];

            for (int d = 0; d < towerComp.DefendersPositions.Length;d++)
            {
                var x = Mathf.Cos(Angle * Mathf.Deg2Rad) * 13;
                var z = Mathf.Sin(Angle * Mathf.Deg2Rad) * 13;

                Angle += 360 / towerComp.DefendersPositions.Length;

                var ent = _world.Value.NewEntity();
                _defenderPool.Value.Add(ent);
                _state.Value.DefendersEntity[d] = ent;
            }

            ref var radiusComp = ref _radiusPool.Value.Add(entity);
            radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(towerID);

            ref var healthComponent = ref _healthPool.Value.Add(entity);
            healthComponent.MaxValue = _state.Value.TowerStorage.GetHealthByID(towerID);
            healthComponent.CurrentValue = healthComponent.MaxValue;

            GameObject upgradePoint = null;
            UpgradePointMB upgradePointMB = null;

            var mainTower = GameObject.Instantiate(_state.Value.TowerStorage.GetTowerPrefabByID(towerID), Vector3.zero, Quaternion.identity);
            upgradePoint = GameObject.Instantiate(_state.Value.InterfaceStorage.UpgradePointPrefab, new Vector3(0, 0, -3), Quaternion.identity);
            upgradePointMB = upgradePoint.GetComponent<UpgradePointMB>();
            upgradePointMB.TowerIndex = 0;

            ref var viewComponent = ref _viewPool.Value.Add(entity);
            viewComponent.GameObject = mainTower;
            Angle = 0;
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

                    upgradePoint = GameObject.Instantiate(_state.Value.InterfaceStorage.UpgradePointPrefab, new Vector3(x, 0, z - 3), Quaternion.identity);
                    upgradePointMB = upgradePoint.GetComponent<UpgradePointMB>();
                    upgradePointMB.TowerIndex = i;

                    Angle += 360 / (_state.Value.TowerCount - 1);
                }
            }
        }
    }
}