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
                        if (!_upgradePool.Has(_state.EntityPlayer))
                        {
                            ref var upgradeComp = ref _upgradePool.Add(_state.EntityPlayer);
                            upgradeComp.TowerIndex = other.GetComponent<UpgradePointMB>().TowerIndex;
                            upgradeComp.Time = 0f;
                            upgradeComp.UpgradeTower = true;
                        }
                        break;
                    }
                case "UpgradePlayerPoint":
                    {
                        if (!_upgradePool.Has(_state.EntityPlayer))
                        {
                            ref var upgradeComp = ref _upgradePool.Add(_state.EntityPlayer);
                            upgradeComp.Time = 0f;
                            upgradeComp.UpgradeTower = false;
                        }
                        break;
                    }
                case "Enemy":
                    {
                        ref var player = ref _state.EntityPlayer;
                        ref var viewComp = ref _viewPool.Get(_state.EntityPlayer);
                        ref var playerComponent = ref _playerPool.Get(player);
                        playerComponent.animator.SetBool("isMining", false);
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
                        //if (_state.DefenseTowerStorage.GetLevelByID(_state.DefenseTowers[other.GetComponent<UpgradePointMB>().TowerIndex]) >= 1)
                        //{
                        //    _viewPool.Get(_state.TowersEntity[other.GetComponent<UpgradePointMB>().TowerIndex]).ResourcesTimer.GetComponent<TimerResourcesMB>().ResourcesDrop(0);
                        //}
                        if (_upgradePool.Has(_state.EntityPlayer))
                        {
                            _upgradePool.Del(_state.EntityPlayer);
                        }
                        break;
                    }
                case "UpgradePlayerPoint":
                    {
                        if(_viewPool.Has(_state.EntityPlayer))
                            _viewPool.Get(_state.EntityPlayer).ResourcesTimer.GetComponent<TimerResourcesMB>().ResourcesDrop(0);
                        if (_upgradePool.Has(_state.EntityPlayer))
                        {
                            _upgradePool.Del(_state.EntityPlayer);
                        }
                        break;
                    }
                case "Enemy":
                    {
                        ref var viewComp = ref _viewPool.Get(_state.EntityPlayer);
                        if (!_fightPool.Has(_state.EntityPlayer)) viewComp.isFight = false;
                        break;
                    }
            }
        }
    }
}
