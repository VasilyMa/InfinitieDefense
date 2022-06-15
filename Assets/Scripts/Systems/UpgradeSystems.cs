using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class UpgradeSystems : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeComponent>> _filter = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<CreateNextTowerEvent> _nextTowerPool = default;

        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(entity);
                if(filterComp.Time == 0)
                {
                    if (_state.Value.RockCount > 0)
                    {
                        //var tr = _state.Value.StoneTransformList[_state.Value.RockCount];
                        GameObject.Destroy(_state.Value.StoneTransformList[_state.Value.RockCount - 1].gameObject);
                        _state.Value.StoneTransformList.Remove(_state.Value.StoneTransformList[_state.Value.RockCount - 1]);
                        _state.Value.RockCount--;

                        _state.Value.UpgradeTower();

                        foreach(var item in _state.Value.CoinTransformList)
                        {
                            item.localPosition = new Vector3(0, item.localPosition.y - 1, 0);
                        }
                    }
                    else
                    {
                        _filter.Pools.Inc1.Del(entity);
                    }
                }
                filterComp.Time += Time.deltaTime * 3f;
                if(filterComp.Time >= 1f)
                {
                    filterComp.Time = 0f;
                }
            }
        }
    }
}