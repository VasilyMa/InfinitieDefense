using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
namespace Client
{
    public class ColliderChecker : MonoBehaviour
    {
        private GameState _state;
        private EcsPool<AddCoinEvent> _coinPool;
        private EcsPool<StoneMiningEvent> _stonePool;
        private EcsPool<UpgradeComponent> _upgradePool;
        private EcsWorld _world;

        public void Init(EcsWorld world, GameState state)
        {
            _state = state;
            _coinPool = world.GetPool<AddCoinEvent>();
            _stonePool = world.GetPool<StoneMiningEvent>();
            _upgradePool = world.GetPool<UpgradeComponent>();
            _world = world;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Coin")
            {
                other.gameObject.tag = "Untagged";
                ref var coinComp = ref _coinPool.Add(_world.NewEntity());
                coinComp.CoinTransform = other.transform;
            }
            else if(other.gameObject.tag == "Stone")
            {
                other.gameObject.tag = "Untagged";
                ref var stoneComp = ref _stonePool.Add(_world.NewEntity());
                stoneComp.StoneTransform = other.transform;
            }
            else if(other.gameObject.tag == "UpgradePoint")
            {
                if (!_upgradePool.Has(_state.EntityPlayer))
                {
                    ref var upgradeComp = ref _upgradePool.Add(_state.EntityPlayer);
                    upgradeComp.Time = 0f;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.tag == "UpgradePoint")
            {
                if(_upgradePool.Has(_state.EntityPlayer))
                {
                    _upgradePool.Del(_state.EntityPlayer);
                }
            }
        }

    }
}
