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
        private EcsPool<CooldownComponent> _cooldownPool;
        private EcsPool<ReloadComponent> _reloadPool;
        private EcsPool<ViewComponent> _viewPool;
        private EcsPool<OreComponent> _orePool;
        private EcsPool<Player> _playerPool;
        private EcsPool<TowerTag> _towerPool;
        private EcsPool<UpgradeCanvasEvent> _canvasEvent;
        private EcsPool<CanvasUpgradeComponent> _upgradeCanvasPool;
        private EcsPool<MainTowerTag> _mainPool;
        private EcsPool<OreEventComponent> _oreEventPool;
        private EcsPool<OreMoveEvent> _moveEventPool;
        private EcsPool<InFightTag> _fightPool;
        private EcsPool<DeadTag> _deadPool;
        private EcsPool<ActivateContextToolEvent> _activateContextToolPool;
        private EcsPool<CoinPickupEvent> _coinPickupEventPool;

        public void Init(EcsWorld world, GameState state)
        {
            _state = state;
            _coinPool = world.GetPool<AddCoinEvent>();
            _stonePool = world.GetPool<StoneMiningEvent>();
            _upgradePool = world.GetPool<UpgradeComponent>();
            _cooldownPool = world.GetPool<CooldownComponent>();
            _reloadPool = world.GetPool<ReloadComponent>();
            _orePool = world.GetPool<OreComponent>();
            _oreEventPool = world.GetPool<OreEventComponent>();
            _moveEventPool = world.GetPool<OreMoveEvent>();
            _playerPool = world.GetPool<Player>();
            _towerPool = world.GetPool<TowerTag>();
            _fightPool = world.GetPool<InFightTag>();
            _mainPool = world.GetPool<MainTowerTag>();
            _canvasEvent = world.GetPool<UpgradeCanvasEvent>();
            _viewPool = world.GetPool<ViewComponent>();
            _deadPool = world.GetPool<DeadTag>();
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
                            //upgradeComp.DelayTime = 0f;
                        }
                        break;
                    }
                case "Enemy":
                    {
                        ref var player = ref _state.EntityPlayer;
                        ref var viewComp = ref _viewPool.Get(_state.EntityPlayer);
                        ref var _player = ref _playerPool.Get(player);
                        _player.animator.SetBool("isMining", false);
                        break;
                    }
                /*case "Ore":
                    {
                        ref var player = ref _state.EntityPlayer;
                        ref var _player = ref _playerPool.Get(player);
                        ref var view = ref _viewPool.Get(player);
                        var filter = _world.Filter<OreComponent>();
                        var ores = _world.GetPool<OreComponent>();
                        foreach (int entity in filter.End()) //to do rework find oreEntity with help EcsInfo on the gameObject
                        {
                            ref OreComponent oreComp = ref ores.Get(entity);
                            if (other.gameObject == oreComp.prefab && !_fightPool.Has(player))
                            {
                                Debug.Log("Майним руду");
                                //_player.playerMB.InitMiningEvent(entity, oreComp.prefab);
                                _player.animator.SetBool("isIdle", false);
                                _player.animator.SetBool("isRun", false);
                                _player.animator.SetBool("isMining", true);
                                ref var activateContextToolComponent = ref _activateContextToolPool.Add(_state.EntityPlayer);
                                activateContextToolComponent.ActiveTool = ContextToolComponent.Tool.pickaxe;
                                break;
                            }
                        }
                        break;
                    }*/
            }
        }
        private void OnTriggerExit(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "UpgradePoint":
                    {
                        if(_state.DefenseTowerStorage.GetLevelByID(_state.DefenseTowers[other.GetComponent<UpgradePointMB>().TowerIndex]) >= 1)
                            _viewPool.Get(_state.TowersEntity[other.GetComponent<UpgradePointMB>().TowerIndex]).ResourcesTimer.GetComponent<TimerResourcesMB>().ResourcesDrop(0);
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
                /*case "Ore":
                    {
                        var playerFilter = _world.Filter<Player>().Exc<InFightTag>().End();
                        foreach (var player in playerFilter)
                        {
                            ref var _player = ref _playerPool.Get(player);
                            _player.animator.SetBool("isMining", false);
                        }
                        var filter = _world.Filter<OreComponent>();
                        foreach (int entity in filter.End())
                        {
                            _oreEventPool.Del(entity);
                        }
                        break;
                    }*/
            }
        }
        /*private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Ore"))
            {
                var playerFilter = _world.Filter<Player>().Exc<InFightTag>().End(); //находим игрока без компонента в бою
                var filter = _world.Filter<OreComponent>();
                var ores = _world.GetPool<OreComponent>();
                foreach (int entity in filter.End())
                {
                    ref OreComponent oreComp = ref ores.Get(entity);
                    foreach (var player in playerFilter)
                    {
                        ref var _player = ref _playerPool.Get(player);
                        ref var view = ref _viewPool.Get(player);
                        if (other.gameObject == oreComp.prefab)
                        {
                            //_player.playerMB.InitMiningEvent(entity, oreComp.prefab);
                            _player.animator.SetBool("isIdle", false);
                            _player.animator.SetBool("isRun", false);
                            _player.animator.SetBool("isMining", true);
                        }
                    }
                }
            }
        }*/

        /*public void StartMining()
        {
            ref var player = ref _state.EntityPlayer;
            ref var _player = ref _playerPool.Get(player);
            ref var view = ref _viewPool.Get(player);
            var filter = _world.Filter<OreComponent>();
            var ores = _world.GetPool<OreComponent>();
            foreach (int entity in filter.End())
            {
                ref OreComponent oreComp = ref ores.Get(entity);
                if (view.CanMining)
                {
                    //_player.playerMB.InitMiningEvent(entity, oreComp.prefab);
                    //_player.animator.SetBool("isIdle", false);
                    //_player.animator.SetBool("isRun", false);
                    _player.animator.SetBool("isMining", true);
                    view.isMining = true;
                    view.isFight = false;
                    Debug.Log("Mining!");
                }
            }
        }*/
    }
}
