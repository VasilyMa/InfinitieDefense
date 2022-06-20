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

        public void Init(EcsWorld world, GameState state)
        {
            _state = state;
            _coinPool = world.GetPool<AddCoinEvent>();
            _stonePool = world.GetPool<StoneMiningEvent>();
            _upgradePool = world.GetPool<UpgradeComponent>();
            _cooldownPool = world.GetPool<CooldownComponent>();
            _reloadPool = world.GetPool<ReloadComponent>();
            _orePool = world.GetPool<OreComponent>();
            _playerPool = world.GetPool<Player>();
            _towerPool = world.GetPool<TowerTag>();
            _mainPool = world.GetPool<MainTowerTag>();
            _canvasEvent = world.GetPool<UpgradeCanvasEvent>();
            _upgradeCanvasPool = world.GetPool<CanvasUpgradeComponent>();
            _world = world;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Coin"))
            {
                other.gameObject.tag = "Untagged";
                ref var coinComp = ref _coinPool.Add(_world.NewEntity());
                coinComp.CoinTransform = other.transform;
            }
            else if (other.gameObject.CompareTag("Stone"))
            {
                other.gameObject.tag = "Untagged";
                ref var stoneComp = ref _stonePool.Add(_world.NewEntity());
                stoneComp.StoneTransform = other.transform;
            }
            else if (other.gameObject.CompareTag("UpgradePoint"))
            {
                if (!_upgradePool.Has(_state.EntityPlayer))
                {
                    ref var upgradeComp = ref _upgradePool.Add(_state.EntityPlayer);
                    upgradeComp.TowerIndex = other.GetComponent<UpgradePointMB>().TowerIndex;
                    upgradeComp.Time = 0f;
                    upgradeComp.UpgradeTower = true;
                    if (_state.RockCount > 0)
                    {
                        var filter = _world.Filter<TowerTag>().Inc<CanvasUpgradeComponent>().End();
                        var towerTag = _world.GetPool<CanvasUpgradeComponent>();
                        foreach (var entity in filter)
                        {
                            ref var towerComp = ref towerTag.Get(entity);
                            if (other.gameObject == towerComp.point)
                            {
                                ref var upgradeEvent = ref _canvasEvent.Add(entity);
                                towerComp.Index = other.GetComponent<UpgradePointMB>().TowerIndex;
                            }
                        }
                    }
                    if (_state.CoinCount > 0)
                    {
                        var mainFilter = _world.Filter<MainTowerTag>().Inc<CanvasUpgradeComponent>().End();
                        var mainTowerTag = _world.GetPool<CanvasUpgradeComponent>();
                        foreach (var entity in mainFilter)
                        {
                            ref var mainTowerComp = ref mainTowerTag.Get(entity);
                            if (other.gameObject == mainTowerComp.point)
                            {
                                ref var upgradeEvent = ref _canvasEvent.Add(entity);
                                mainTowerComp.Index = other.GetComponent<UpgradePointMB>().TowerIndex;
                            }
                        }
                    }
                }
            }
            else if (other.gameObject.tag == "UpgradePlayerPoint")
            {
                if (!_upgradePool.Has(_state.EntityPlayer))
                {
                    ref var upgradeComp = ref _upgradePool.Add(_state.EntityPlayer);
                    upgradeComp.Time = 0f;
                    upgradeComp.UpgradeTower = false;

                }
            }
            else if (other.gameObject.CompareTag("Ore"))
            {
                ref var player = ref _state.EntityPlayer;
                ref var _player = ref _playerPool.Get(player);
                var filter = _world.Filter<OreComponent>();
                var ores = _world.GetPool<OreComponent>();
                foreach (int entity in filter.End())
                {
                    ref OreComponent oreComp = ref ores.Get(entity);
                    if (other.gameObject == oreComp.prefab)
                    {
                        _player.playerMB.InitMiningEvent(entity, oreComp.prefab);
                        _player.animator.SetBool("isIdle", false);
                        _player.animator.SetBool("isRun", false);
                        _player.animator.SetBool("isMining", true);
                        Debug.Log("Mining!");
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("UpgradePoint"))
            {
                if (_upgradePool.Has(_state.EntityPlayer))
                {
                    _upgradePool.Del(_state.EntityPlayer);
                }
            }
            else if (other.gameObject.tag == "UpgradePlayerPoint")
            {
                if (_upgradePool.Has(_state.EntityPlayer))
                {
                    _upgradePool.Del(_state.EntityPlayer);
                }
            }
            if (other.gameObject.CompareTag("Ore"))
            {
                ref var player = ref _state.EntityPlayer;
                ref var _player = ref _playerPool.Get(player);
                _player.animator.SetBool("isMining", false);
            }
        }
        /*private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("UpgradePoint"))
            {
                var filter = _world.Filter<TowerTag>();
                var towerTag = _world.GetPool<CanvasUpgradeComponent>();
                foreach (var entity in filter.End())
                {
                    ref var towerComp = ref towerTag.Get(entity);
                    if (other.gameObject == towerComp.point)
                    {
                        ref var upgradeEvent = ref _canvasEvent.Add(entity);
                        towerComp.Index = other.GetComponent<UpgradePointMB>().TowerIndex;
                    }
                }
            }
        }*/
    }
}
