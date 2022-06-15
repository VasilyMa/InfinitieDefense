using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class ShipArrivalMonoBehavior : MonoBehaviour
    {
        private EcsWorldInject _world;
        //private EcsWorld _world;
        private EcsPool<ShipArrivalEvent> _shipArrivalEventPool;
        private EcsPool<InactiveTag> _inactivePool;

        [SerializeField] private int _shipEntity;
        [SerializeField] private int _shipNumber;

        public void Init(EcsWorldInject world)
        {
            _shipArrivalEventPool = world.Value.GetPool<ShipArrivalEvent>();
            _inactivePool = world.Value.GetPool<InactiveTag>();
            _world = world;
        }

        private void OnTriggerEnter(Collider land)
        {
            Debug.Log(land.gameObject.name);

            if (_inactivePool.Has(_shipEntity))
            {
                return;
            }

            ref var shipArrivalEvent = ref _shipArrivalEventPool.Add(_world.Value.NewEntity());
            shipArrivalEvent.ShipEntity = _shipEntity;

        }

        public void SetEntity(int entity)
        {
            _shipEntity = entity;
        }

        public int GetShipEntity()
        {
            return _shipEntity;
        }
        public int GetShipNumber()
        {
            return _shipNumber;
        }
    }
}
