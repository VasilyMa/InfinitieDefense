using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
namespace Client
{
    public class ColliderChecker : MonoBehaviour 
    {
        private GameState _state;
        private EcsWorld _world;
        private EcsPool<AddCoinEvent> _coinPool;
        private EcsPool<StoneMiningEvent> _stonePool;
        private EcsPool<UpgradeComponent> _upgradePool;
        private EcsPool<ViewComponent> _viewPool;
        private EcsPool<Player> _playerPool;
        private EcsPool<InFightTag> _fightPool;
        private EcsPool<DeadTag> _deadPool;
        private EcsPool<ActivateContextToolEvent> _activateContextToolPool;
        private EcsPool<CoinPickupEvent> _coinPickupEventPool;
        private EcsPool<CanvasUpgradeComponent> _upgradeCanvasPool;
        private EcsPool<UpgradeTimerEvent> _timerPool;
        private EcsPool<UpgradePlayerPointComponent> _playerUpgradePool;
        private EcsPool<TutorialComponent> _tutorPool;

        public void Init(EcsWorld world, GameState state)
        {
            _state = state;
            _coinPool = world.GetPool<AddCoinEvent>();
            _stonePool = world.GetPool<StoneMiningEvent>();
            _upgradePool = world.GetPool<UpgradeComponent>();
            _fightPool = world.GetPool<InFightTag>();
            _viewPool = world.GetPool<ViewComponent>();
            _deadPool = world.GetPool<DeadTag>();
            _playerPool = world.GetPool<Player>();
            _upgradeCanvasPool = world.GetPool<CanvasUpgradeComponent>();
            _activateContextToolPool = world.GetPool<ActivateContextToolEvent>();
            _coinPickupEventPool = world.GetPool<CoinPickupEvent>();
            _timerPool = world.GetPool<UpgradeTimerEvent>();
            _playerUpgradePool = world.GetPool<UpgradePlayerPointComponent>();
            _tutorPool = world.GetPool<TutorialComponent>();
            _world = world;
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "Coin":
                    {
                        if (_deadPool.Has(_state.EntityPlayer))
                        {
                            break;
                        }
                        //if(_state.Saves.TutorialStage < 3)
                        //{
                        //    break;
                        //}
                        other.gameObject.tag = "Untagged";
                        ref var coinComp = ref _coinPickupEventPool.Add(_world.NewEntity());
                        coinComp.CoinObject = other.gameObject;
                        coinComp.Speed = 30f;
                        //ref var coinComp = ref _coinPool.Add(_world.NewEntity());
                        //coinComp.CoinTransform = other.transform;
                        break;
                    }
                case "Stone":
                    {
                        if (_deadPool.Has(_state.EntityPlayer))
                        {
                            break;
                        }
                        other.gameObject.tag = "Untagged";
                        ref var stoneComp = ref _stonePool.Add(_world.NewEntity());
                        stoneComp.StoneTransform = other.transform;
                        break;
                    }
                case "UpgradePoint":
                    {
                        if (other.GetComponent<UpgradePointMB>().TowerIndex == 0)
                        {
                            if (!_upgradePool.Has(_state.EntityPlayer) && _state.CoinCount > 0)
                            {
                                ref var upgradeComp = ref _upgradePool.Add(_state.EntityPlayer);
                                upgradeComp.TowerIndex = other.GetComponent<UpgradePointMB>().TowerIndex;
                                upgradeComp.DelayAfterUpgrade = 0f;
                                upgradeComp.UpgradeTower = true;
                                ref var timerComp = ref _timerPool.Add(_world.NewEntity());
                                timerComp.Entity = _state.TowersEntity[upgradeComp.TowerIndex];
                                timerComp.TimeToUpgrade = 0;
                            }
                        }
                        else if (other.GetComponent<UpgradePointMB>().TowerIndex > 0)
                        {
                            if (!_upgradePool.Has(_state.EntityPlayer) && _state.RockCount > 0)
                            {
                                ref var upgradeComp = ref _upgradePool.Add(_state.EntityPlayer);
                                upgradeComp.TowerIndex = other.GetComponent<UpgradePointMB>().TowerIndex;
                                upgradeComp.DelayAfterUpgrade = 0f;
                                upgradeComp.UpgradeTower = true;
                                ref var timerComp = ref _timerPool.Add(_world.NewEntity());
                                timerComp.Entity = _state.TowersEntity[upgradeComp.TowerIndex];
                                timerComp.TimeToUpgrade = 0;
                            }
                        }
                        break;
                    }
                case "UpgradePlayerPoint":
                    {
                        if (!_upgradePool.Has(_state.EntityPlayer) && _state.CoinCount > 0)
                        {
                            ref var upgradeComp = ref _upgradePool.Add(_state.EntityPlayer);
                            upgradeComp.DelayAfterUpgrade = 0f;
                            upgradeComp.UpgradeTower = false;
                            ref var timerComp = ref _timerPool.Add(_world.NewEntity());
                            timerComp.Entity = _state.EntityPlayer;
                            timerComp.TimeToUpgrade = 0;
                        }
                        break;
                    }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "UpgradePoint":
                    {
                        _upgradeCanvasPool.Get(_state.TowersEntity[other.GetComponent<UpgradePointMB>().TowerIndex]).timerResources.ResourcesDrop(0);
                        var filter = _world.Filter<UpgradeTimerEvent>().End();
                        foreach (var entity in filter)
                        {
                            _timerPool.Del(entity);
                        }
                        if (_upgradePool.Has(_state.EntityPlayer))
                        {
                            _upgradePool.Del(_state.EntityPlayer);
                        }
                        break;
                    }
                case "UpgradePlayerPoint":
                    {
                        _upgradeCanvasPool.Get(_state.EntityPlayer).timerResources.ResourcesDrop(0);
                        var filter = _world.Filter<UpgradeTimerEvent>().End();
                        foreach (var entity in filter)
                        {
                            _timerPool.Del(entity);
                        }
                        if (_upgradePool.Has(_state.EntityPlayer))
                        {
                            _upgradePool.Del(_state.EntityPlayer);
                        }
                        break;
                    }
            }
        }
    }
}
