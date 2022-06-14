using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class UpgradeSystems : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeComponent>> _filter = default;
        readonly EcsPoolInject<Player> _playerPool = default;

        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(entity);
                if(filterComp.Time == 0)
                {
                    if (_state.Value.RockCount > 0)
                    {
                        GameObject.Destroy(_state.Value.StoneTransformList[_state.Value.RockCount].gameObject);
                        _state.Value.StoneTransformList.Remove(_state.Value.StoneTransformList[_state.Value.RockCount]);
                        _state.Value.RockCount--;
                        
                        

                    }
                }
            }
        }
    }
}