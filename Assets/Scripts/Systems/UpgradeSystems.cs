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
        readonly EcsPoolInject<Player> _playerPool = default;
        //readonly EcsPoolInject<CreateNextTowerEvent> _nextTowerPool = default;
        readonly EcsPoolInject<InterfaceComponent> _intPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsFilterInject<Inc<UpgradePlayerPointComponent>> _filterPoint = default;
        readonly EcsPoolInject<CanvasUpgradeComponent> _canvasFilter = default;
        readonly EcsPoolInject<VibrationEvent> _vibrationEventPool = default;
        readonly EcsPoolInject<UpgradeComponent> _upgradeEventPool = default;
        readonly EcsFilterInject<Inc<TutorialComponent>> _tutorPool = default;
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

                int neededResource = 0;

                if (upgradeComponent.UpgradeTower) //если апгрейдим башни
                {
                    if (upgradeComponent.TowerIndex == 0) //главная башня
                    {
                        neededResource = _state.Value.CoinCount;
                    }
                    else //защитные башни
                    {
                        neededResource = _state.Value.RockCount;
                    }

                }
                else // если апгрейдим игрока
                {
                    neededResource = _state.Value.CoinCount;
                }

                if (upgradeComponent.DelayBeforUpgrade < _state.Value.DelayBeforUpgrade)
                {
                    upgradeComponent.DelayBeforUpgrade += Time.deltaTime;
                    return;
                }

                if (upgradeComponent.DelayAfterUpgrade == 0)
                {
                    if (neededResource > 0)
                    {
                        if (upgradeComponent.UpgradeTower)
                        {
                            if (upgradeComponent.TowerIndex == 0)
                            {
                                //GameObject.Destroy(_state.Value.CoinTransformList[_state.Value.CoinCount - 1].gameObject);
                                //_state.Value.CoinTransformList.Remove(_state.Value.CoinTransformList[_state.Value.CoinCount - 1]);
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

                                //foreach (var item in _tutorPool.Value) //этап тутора
                                //{
                                //    ref var upgradePointComp = ref _canvasFilter.Value.Get(_state.Value.TowersEntity[filterComp.TowerIndex]);
                                //    ref var tutorComp = ref _tutorPool.Pools.Inc1.Get(item);
                                //    if (_state.Value.Saves.TutorialStage <= 5)
                                //    {
                                //        GameObject.Destroy(tutorComp.TutorialCursor);
                                //        tutorComp.TutorialStage = 6;
                                //        _state.Value.Saves.TutorialStage = 6;
                                //        _state.Value.Saves.SaveTutorial(6);
                                //        upgradePointComp.point.SetActive(false);
                                //        _filter.Pools.Inc1.Del(_state.Value.EntityPlayer);
                                //        foreach (var point in _filterPoint.Value)
                                //        {
                                //            _filterPoint.Pools.Inc1.Get(point).Point.SetActive(true);
                                //        }
                                //    }
                                //}

                            }
                            else
                            {
                                GameObject.Destroy(_state.Value.StoneTransformList[_state.Value.RockCount - 1].gameObject);
                                _state.Value.StoneTransformList.Remove(_state.Value.StoneTransformList[_state.Value.RockCount - 1]);
                                _state.Value.RockCount--;
                                _vibrationEventPool.Value.Add(_world.Value.NewEntity()).Vibration = VibrationEvent.VibrationType.SoftImpact;

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

                            }
                            _state.Value.UpgradeTower(upgradeComponent.TowerIndex);
                        }
                        else
                        {
                            //GameObject.Destroy(_state.Value.CoinTransformList[_state.Value.CoinCount - 1].gameObject);
                            //_state.Value.CoinTransformList.Remove(_state.Value.CoinTransformList[_state.Value.CoinCount - 1]);
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
                            //viewComp.Level.UpdateLevel(_state.Value.PlayerStorage.GetLevelByID(_state.Value.CurrentPlayerID));
                            foreach (var item in _filterPoint.Value)
                            {
                                _filterPoint.Pools.Inc1.Get(item).Point.GetComponent<PlayerUpgradePointMB>().
                                    UpdateLevelInfo(_state.Value.PlayerStorage.
                                    GetUpgradeByID(_state.Value.CurrentPlayerID), _state.Value.PlayerExperience);
                            }

                            //ref var upgradePointComp = ref _canvasFilter.Value.Get(_state.Value.TowersEntity[0]);
                            //foreach (var item in _tutorPool.Value)
                            //{
                            //    ref var tutorComp = ref _tutorPool.Pools.Inc1.Get(item);
                            //    GameObject.Destroy(tutorComp.TutorialCursor);
                            //    if (tutorComp.TutorialStage <= 7)
                            //    {
                            //        tutorComp.TutorialStage = 8;
                            //        _state.Value.Saves.TutorialStage = 8;
                            //        _state.Value.Saves.SaveTutorial(8);
                            //        upgradePointComp.point.SetActive(true);
                            //    }
                            //}
                        }
                        //viewComp.DropItemParticleSystem.Stop();
                        viewComp.DropItemParticleSystem.Play();
                    }
                    else
                    {
                        _upgradeEventFilter.Pools.Inc1.Del(eventEntity);
                    }
                }
                upgradeComponent.DelayAfterUpgrade += Time.deltaTime;
                if (upgradeComponent.DelayAfterUpgrade >= _state.Value.DelayAfterUpgrade)
                {
                    upgradeComponent.DelayAfterUpgrade = 0f;
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