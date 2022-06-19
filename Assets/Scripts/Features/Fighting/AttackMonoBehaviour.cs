using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class AttackMonoBehaviour : MonoBehaviour
    {
        private EcsWorldInject _world;

        private EcsPool<DamagingEventComponent> _damagingEventPool;
        private EcsPool<Targetable> _targetablePool;

        [SerializeField] private int _unitEntity;
        [SerializeField] private int _targetEntity;
        [SerializeField] private GameObject _targetObject;
        [SerializeField] private EcsInfoMB _ecsInfoMB;
        [SerializeField] private float _damageValue;

        public void Start()
        {
            _ecsInfoMB = gameObject.GetComponent<EcsInfoMB>();
        }

        public void Init(EcsWorldInject world)
        {
            _world = world;
            _damagingEventPool = world.Value.GetPool<DamagingEventComponent>();
            _targetablePool = world.Value.GetPool<Targetable>();
        }

        public void SetEntity(int entity)
        {
            _unitEntity = entity;
        }

        public void SetTargetInfo(int entity, GameObject gameObject)
        {
            _targetEntity = entity;
            _targetObject = gameObject;
        }

        public void SetDamageValue(float damageValue)
        {
            _damageValue = damageValue;
        }

        public int GetUnitEntity()
        {
            return _unitEntity;
        }

        public void DealDamageEvent()
        {
            if (_targetObject != null)
            {
                ref var damagingEventComponent = ref _damagingEventPool.Add(_world.Value.NewEntity());
                damagingEventComponent.TargetingEntity = _targetEntity;
                damagingEventComponent.DamageValue = _damageValue;
                damagingEventComponent.DamagingEntity = _unitEntity;
            }
        }
    }
}
