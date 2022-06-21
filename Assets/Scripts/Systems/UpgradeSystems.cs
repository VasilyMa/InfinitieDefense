using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class UpgradeSystems : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeComponent>> _filter = default;
        readonly EcsFilterInject<Inc<CanvasUpgradeComponent, UpgradeCanvasEvent>> _canvasFilter = default;
        readonly EcsFilterInject<Inc<CanvasUpgradeComponent, MainTowerTag, UpgradeCanvasEvent>> _mainCanvasFilter = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<CreateNextTowerEvent> _nextTowerPool = default;
        readonly EcsPoolInject<InterfaceComponent> _intPool = default;
        readonly EcsPoolInject<CanvasUpgradeComponent> _upgradePool = default;
        private int TowerEntity;
        private int MainTowerEntity;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(entity);
                ref var intComp = ref _intPool.Value.Get(_state.Value.EntityInterface);
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

                if(filterComp.Time == 0)
                {
                    if (neededResource > 0)
                    {
                        if (filterComp.UpgradeTower)
                        {
                            if (filterComp.TowerIndex == 0)
                            {
                                foreach (var mainEntity in _mainCanvasFilter.Value)
                                {
                                    ref var upgradeComp = ref _upgradePool.Value.Get(mainEntity);
                                    upgradeComp.upgrade.UpdateUpgradePoint(_state.Value.TowersUpgrade[0], _state.Value.TowerStorage.GetUpgradeByID(_state.Value.DefenseTowers[upgradeComp.Index]));
                                    MainTowerEntity = mainEntity;
                                }
                                GameObject.Destroy(_state.Value.CoinTransformList[_state.Value.CoinCount - 1].gameObject);
                                _state.Value.CoinTransformList.Remove(_state.Value.CoinTransformList[_state.Value.CoinCount - 1]);
                                _state.Value.CoinCount--;
                                intComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateGold();
                            }
                            else
                            {
                                foreach (var towerEntity in _canvasFilter.Value)
                                {
                                    ref var upgradeComp = ref _upgradePool.Value.Get(towerEntity);
                                    upgradeComp.upgrade.UpdateUpgradePoint(_state.Value.TowersUpgrade[upgradeComp.Index], _state.Value.DefenseTowerStorage.GetUpgradeByID(_state.Value.DefenseTowers[upgradeComp.Index]));
                                    TowerEntity = towerEntity;
                                }
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
                    }
                    else
                    {
                        _filter.Pools.Inc1.Del(entity);
                    }
                    _canvasFilter.Pools.Inc2.Del(TowerEntity);
                    _mainCanvasFilter.Pools.Inc3.Del(MainTowerEntity);
                }
                filterComp.Time += Time.deltaTime * 3f;
                if(filterComp.Time >= 1f)
                {
                    filterComp.Time = 0f;
                }
            }
            void RelocateCoinInResourceHolder()
            {
                foreach(var item in _state.Value.CoinTransformList)
                {
                    item.localPosition = new Vector3(0, item.localPosition.y - 1, 0);
                }
            }
        }
    }
}