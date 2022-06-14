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
        private EcsWorld _world;

        public void Init(EcsWorld world, GameState state)
        {
            _state = state;
            _coinPool = world.GetPool<AddCoinEvent>();
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
        }
    }
}
