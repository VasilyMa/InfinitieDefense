using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class NewTowersCircleSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<NewTowerCircleEvent>> _f = default;
        readonly EcsFilterInject<Inc<TowerTag>> _filter = default;
        public void Run (EcsSystems systems) {
            foreach(var _ in _f.Value)
            {
                foreach(var entity in _filter.Value)
                {
                    ref var towerComp = ref _filter.Pools.Inc1.Get(entity);
                    if(towerComp.Circle == _state.Value.Saves.Circle)
                    {
                        towerComp.UpgradePointGO.SetActive(true);
                    }
                }
                _f.Pools.Inc1.Del(_);
            }
        }
    }
}