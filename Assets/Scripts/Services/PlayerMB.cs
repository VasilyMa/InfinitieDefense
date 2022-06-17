using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
namespace Client
{
    public class PlayerMB : MonoBehaviour
    {
        private GameState _state;
        private EcsPool<OreEventComponent> _orePool;
        private EcsWorld _world;
        private int _entity;
        private GameObject _target;

        public void Init(EcsWorld world, GameState state)
        {
            _state = state;
            _orePool = world.GetPool<OreEventComponent>();
            _world = world;
        }
        public void InitMiningEvent(int entity, GameObject target)
        {
            _target = target;
            _entity = entity;
        }
        public void MiningEvent()
        {
            if (_target != null)
            {
                ref var oreEvent = ref _orePool.Add(_entity);
            }
        }
    }
}
