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

                int neededResource = 0;

                if(filterComp.TowerIndex == 0)
                {
                    if(_state.Value.TowerStorage.GetIsLastByID(_state.Value.DefenseTowers[filterComp.TowerIndex]))
                    {
                        _filter.Pools.Inc1.Del(entity);
                        return;
                    }
                    neededResource = _state.Value.CoinCount;
                }
                else
                {
                    if(_state.Value.DefenseTowerStorage.GetIsLastByID(_state.Value.DefenseTowers[filterComp.TowerIndex]))
                    {
                        _filter.Pools.Inc1.Del(entity);
                        return;
                    }
                    neededResource = _state.Value.RockCount;
                }

                

                if(filterComp.Time == 0)
                {
                    if (neededResource > 0)
                    {
                        

                        //todo переделать на апгрейд главной башни
                        if (filterComp.TowerIndex == 0)
                        {
                            GameObject.Destroy(_state.Value.CoinTransformList[_state.Value.CoinCount - 1].gameObject);
                            _state.Value.CoinTransformList.Remove(_state.Value.CoinTransformList[_state.Value.CoinCount - 1]);
                            _state.Value.CoinCount--;
                        }
                        else
                        {
                            GameObject.Destroy(_state.Value.StoneTransformList[_state.Value.RockCount - 1].gameObject);
                            _state.Value.StoneTransformList.Remove(_state.Value.StoneTransformList[_state.Value.RockCount - 1]);
                            _state.Value.RockCount--;

                            foreach(var item in _state.Value.CoinTransformList)
                            {
                                item.localPosition = new Vector3(0, item.localPosition.y - 1, 0);
                            }
                        }

                        _state.Value.UpgradeTower(filterComp.TowerIndex);
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