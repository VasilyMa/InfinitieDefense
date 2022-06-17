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
            else if(other.gameObject.CompareTag("Stone"))
            {
                other.gameObject.tag = "Untagged";
                ref var stoneComp = ref _stonePool.Add(_world.NewEntity());
                stoneComp.StoneTransform = other.transform;
            }
            else if(other.gameObject.CompareTag("UpgradePoint"))
            {
                if (!_upgradePool.Has(_state.EntityPlayer))
                {
                    ref var upgradeComp = ref _upgradePool.Add(_state.EntityPlayer);
                    upgradeComp.TowerIndex = other.GetComponent<UpgradePointMB>().TowerIndex;
                    upgradeComp.Time = 0f;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.CompareTag("UpgradePoint"))
            {
                if(_upgradePool.Has(_state.EntityPlayer))
                {
                    _upgradePool.Del(_state.EntityPlayer);
                }
            }
            if (other.gameObject.CompareTag("Ore"))
            {
                if (_cooldownPool.Get(_state.EntityPlayer).currentValue == 0)
                    _playerPool.Get(_state.EntityPlayer).animator.SetBool("isIdle", true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Ore"))
            {
                ref var player = ref _state.EntityPlayer;
                ref var _player = ref _playerPool.Get(player);
                ref var cooldownComp = ref _cooldownPool.Get(player);
                var filter = _world.Filter<OreComponent>();
                var ores = _world.GetPool<OreComponent>();
                if (cooldownComp.currentValue == 0)
                {
                    cooldownComp.currentValue = cooldownComp.maxValue;
                    foreach (int entity in filter.End())
                    {
                        ref OreComponent oreComp = ref ores.Get(entity);
                        if (other.gameObject == oreComp.prefab)
                        {
                            _player.playerMB.InitMiningEvent(entity, oreComp.prefab);
                            _player.animator.SetBool("isIdle", false);
                            _player.animator.SetBool("isRun", false);
                            _player.animator.SetTrigger("Mining");
                            ref var reloadComp = ref _reloadPool.Add(player);
                            Debug.Log("Mining!");
                        }
                    }
                }
                else
                {
                    _player.animator.SetBool("isIdle", true);
                }
            }
        }
    }
}
