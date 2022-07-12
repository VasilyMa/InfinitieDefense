using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class NewTowersCircleSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<NewTowerCircleEvent>> _newTowerCircleFilter = default;
        readonly EcsFilterInject<Inc<TowerTag>, Exc<MainTowerTag>> _towerFilter = default;
        readonly EcsPoolInject<TowerTag> _towerPool = default;
        public void Run (EcsSystems systems) {
            foreach(var eventEntity in _newTowerCircleFilter.Value)
            {
                foreach(var towerEntity in _towerFilter.Value)
                {
                    ref var towerComp = ref _towerFilter.Pools.Inc1.Get(towerEntity);
                    ref var mainTowerComp = ref _towerPool.Value.Get(_state.Value.TowersEntity[0]);

                    if (towerComp.CircleRadiusLevel <= mainTowerComp.Level - 1)
                    {
                        towerComp.UpgradePointGO.SetActive(true);
                    }
                }
                _newTowerCircleFilter.Pools.Inc1.Del(eventEntity);
            }
        }
    }
}