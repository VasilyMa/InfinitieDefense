using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class UpgradeSystems : IEcsRunSystem
    {
        readonly EcsSharedInject<GameState> _state = default;

        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<UpgradeComponent>> _upgradeEventFilter = default;
        readonly EcsFilterInject<Inc<UpgradePlayerPointComponent>> _filterPoint = default;
        readonly EcsFilterInject<Inc<TutorialComponent>> _tutorPool = default;

        readonly EcsPoolInject<Player> _playerPool = default;
        //readonly EcsPoolInject<CreateNextTowerEvent> _nextTowerPool = default;
        readonly EcsPoolInject<InterfaceComponent> _intPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<CanvasUpgradeComponent> _canvasFilter = default;
        readonly EcsPoolInject<VibrationEvent> _vibrationEventPool = default;
        readonly EcsPoolInject<UpgradeComponent> _upgradeEventPool = default;
        readonly EcsPoolInject<TowerRecoveryEvent> _towerRecoveryPool = default;

        private string MainTower;
        private string DefenceTower;
        private string Player;

        public void Run (EcsSystems systems)
        {
            foreach(var eventEntity in _upgradeEventFilter.Value)
            {
                ref var upgradeComponent = ref _upgradeEventPool.Value.Get(eventEntity);
                ref var playerComp = ref _playerPool.Value.Get(eventEntity);
                ref var intComp = ref _intPool.Value.Get(_state.Value.EntityInterface);
                ref var viewComp = ref _viewPool.Value.Get(eventEntity);

                if (intComp._joystick.Horizontal != 0 || intComp._joystick.Vertical != 0)
                {
                    upgradeComponent.DelayBeforUpgrade = 0f;
                    upgradeComponent.DelayAfterUpgrade = 0f;
                    continue;
                }

                int currentResource = 0;
                int needToUpgrade = 0;
                string upgradOf = "";

                if (upgradeComponent.UpgradeTower) //если апгрейдим башни
                {
                    if (upgradeComponent.TowerIndex == 0) //главная башня
                    {
                        currentResource = _state.Value.CoinCount;
                        needToUpgrade = _state.Value.TowerStorage.GetUpgradeByID(_state.Value.DefenseTowers[upgradeComponent.TowerIndex]);
                        upgradOf = nameof(MainTower);
                    }
                    else //защитные башни
                    {
                        currentResource = _state.Value.RockCount;
                        needToUpgrade = _state.Value.DefenseTowerStorage.GetUpgradeByID(_state.Value.DefenseTowers[upgradeComponent.TowerIndex]);
                        upgradOf = nameof(DefenceTower);
                    }
                }
                else // если апгрейдим игрока
                {
                    currentResource = _state.Value.CoinCount;
                    needToUpgrade = _state.Value.PlayerStorage.GetUpgradeByID(_state.Value.CurrentPlayerID);
                    upgradOf = nameof(Player);
                }

                if (upgradeComponent.DelayBeforUpgrade < _state.Value.DelayBeforUpgrade)
                {
                    upgradeComponent.DelayBeforUpgrade += Time.deltaTime;
                    continue;
                }

                float delayPerResource = _state.Value.DelayAfterUpgrade / needToUpgrade;

                if (upgradeComponent.DelayAfterUpgrade < delayPerResource)
                {
                    upgradeComponent.DelayAfterUpgrade += Time.deltaTime;
                    continue;
                }

                if (currentResource <= 0)
                {
                    _upgradeEventPool.Value.Del(eventEntity);
                    continue;
                }

                switch (upgradOf)
                {
                    case nameof(MainTower):
                        {
                            _state.Value.CoinCount--;
                            _vibrationEventPool.Value.Add(_world.Value.NewEntity()).Vibration = VibrationEvent.VibrationType.SoftImpact;

                            if (_state.Value.CoinCount > 0)
                            {
                                intComp.resourcePanelMB.EnableGoldPanel();
                            }
                            else
                            {
                                intComp.resourcePanelMB.DisableGoldPanel();
                            }

                            _towerRecoveryPool.Value.Add(_world.Value.NewEntity()).TowerEntity = _state.Value.TowersEntity[upgradeComponent.TowerIndex];

                            intComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateGold();
                            _state.Value.UpgradeTower(upgradeComponent.TowerIndex);
                            break;
                        }

                    case nameof(DefenceTower):
                        {
                            GameObject.Destroy(_state.Value.StoneTransformList[_state.Value.RockCount - 1].gameObject);
                            _state.Value.StoneTransformList.Remove(_state.Value.StoneTransformList[_state.Value.RockCount - 1]);
                            _state.Value.RockCount--;
                            _vibrationEventPool.Value.Add(_world.Value.NewEntity()).Vibration = VibrationEvent.VibrationType.SoftImpact;

                            _towerRecoveryPool.Value.Add(_world.Value.NewEntity()).TowerEntity = _state.Value.TowersEntity[upgradeComponent.TowerIndex];

                            if (_state.Value.RockCount > 0)
                            {
                                intComp.resourcePanelMB.EnableStonePanel();
                            }
                            else
                            {
                                intComp.resourcePanelMB.DisableStonePanel();
                            }

                            intComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateStone();
                            RelocateCoinInResourceHolder();

                            foreach (var item in _tutorPool.Value)
                            {
                                ref var tutorComp = ref _tutorPool.Pools.Inc1.Get(item);
                                if (_state.Value.Saves.TutorialStage <= 11)
                                {
                                    GameObject.Destroy(tutorComp.TutorialCursor);
                                    tutorComp.TutorialStage = 12;
                                    _state.Value.Saves.TutorialStage = 12;
                                    _state.Value.Saves.SaveTutorial(12);
                                }
                            }
                            _state.Value.UpgradeTower(upgradeComponent.TowerIndex);
                            break;
                        }

                    case nameof(Player):
                        {
                            _state.Value.CoinCount--;
                            _vibrationEventPool.Value.Add(_world.Value.NewEntity()).Vibration = VibrationEvent.VibrationType.SoftImpact;

                            if (_state.Value.CoinCount > 0)
                            {
                                intComp.resourcePanelMB.EnableGoldPanel();
                            }
                            else
                            {
                                intComp.resourcePanelMB.DisableGoldPanel();
                            }

                            intComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateGold();
                            _state.Value.UpgradePlayer();

                            foreach (var item in _filterPoint.Value)
                            {
                                _filterPoint.Pools.Inc1.Get(item).Point.GetComponent<PlayerUpgradePointMB>().
                                    UpdateLevelInfo(_state.Value.PlayerStorage.
                                    GetUpgradeByID(_state.Value.CurrentPlayerID), _state.Value.PlayerExperience);
                            }
                            break;
                        }
                }

                viewComp.DropItemParticleSystem.Play();

                upgradeComponent.DelayAfterUpgrade = 0f;
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