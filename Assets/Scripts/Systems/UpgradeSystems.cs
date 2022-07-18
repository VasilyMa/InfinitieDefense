using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class UpgradeSystems : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeComponent>> _filter = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        //readonly EcsPoolInject<CreateNextTowerEvent> _nextTowerPool = default;
        readonly EcsPoolInject<InterfaceComponent> _intPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(entity);
                ref var intComp = ref _intPool.Value.Get(_state.Value.EntityInterface);
                ref var viewComp = ref _viewPool.Value.Get(entity);
                int neededResource = 0;
                if (filterComp.UpgradeTower) //если апгрейдим башни
                {
                    if (filterComp.TowerIndex == 0) //главная башня
                    {
                        if (_state.Value.TowerStorage.GetIsLastByID(_state.Value.DefenseTowers[filterComp.TowerIndex]))
                        {
                            _filter.Pools.Inc1.Del(entity);
                            return;
                        }
                        neededResource = _state.Value.CoinCount;
                    }
                    else //защитные башни
                    {
                        if (_state.Value.DefenseTowerStorage.GetIsLastByID(_state.Value.DefenseTowers[filterComp.TowerIndex]))
                        {
                            _filter.Pools.Inc1.Del(entity);
                            return;
                        }
                        neededResource = _state.Value.RockCount;
                    }
                    
                }
                else // если апгрейдим игрока
                {
                    if (_state.Value.PlayerStorage.GetIsLastByID(_state.Value.CurrentPlayerID))
                    {
                        _filter.Pools.Inc1.Del(entity);
                        return;
                    }
                    neededResource = _state.Value.CoinCount;
                }

                if (filterComp.DelayTime < 2f)
                {
                    filterComp.DelayTime += Time.deltaTime * 2f;
                    return;
                }

                if (filterComp.Time == 0)
                {
                    if (neededResource > 0)
                    {
                        if (filterComp.UpgradeTower)
                        {
                            if (filterComp.TowerIndex == 0)
                            {
                                GameObject.Destroy(_state.Value.CoinTransformList[_state.Value.CoinCount - 1].gameObject);
                                _state.Value.CoinTransformList.Remove(_state.Value.CoinTransformList[_state.Value.CoinCount - 1]);
                                _state.Value.CoinCount--;
                                intComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateGold();
                            }
                            else
                            {
                                GameObject.Destroy(_state.Value.StoneTransformList[_state.Value.RockCount - 1].gameObject);
                                _state.Value.StoneTransformList.Remove(_state.Value.StoneTransformList[_state.Value.RockCount - 1]);
                                _state.Value.RockCount--;
                                intComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateStone();
                                RelocateCoinInResourceHolder();
                            }
                            _state.Value.UpgradeTower(filterComp.TowerIndex);
                        }
                        else
                        {
                            GameObject.Destroy(_state.Value.CoinTransformList[_state.Value.CoinCount - 1].gameObject);
                            _state.Value.CoinTransformList.Remove(_state.Value.CoinTransformList[_state.Value.CoinCount - 1]);
                            _state.Value.CoinCount--;
                            intComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateGold();
                            _state.Value.UpgradePlayer();
                        }
                        //viewComp.DropItemParticleSystem.Stop();
                        viewComp.DropItemParticleSystem.Play();
                    }
                    else
                    {
                        _filter.Pools.Inc1.Del(entity);
                    }
                }
                filterComp.Time += Time.deltaTime * 2f;
                if(filterComp.Time >= 1f)
                {
                    filterComp.Time = 0f;
                }
            }
            void RelocateCoinInResourceHolder()
            {
                foreach(var item in _state.Value.CoinTransformList)
                {
                    item.localPosition = new Vector3(0, item.localPosition.y - 0.6f, 0);
                }
            }
        }
    }
}