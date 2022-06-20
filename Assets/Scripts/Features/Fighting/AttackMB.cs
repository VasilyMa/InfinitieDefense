using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class AttackMB : MonoBehaviour
    {
        [SerializeField] private EcsInfoMB _ecsInfoMB;

        private Animator _animator;

        private EcsWorldInject _world;

        private EcsPool<DamagingEvent> _damagingEventPool;
        private EcsPool<DamageComponent> _damagePool;
        private EcsPool<Targetable> _targetablePool;


        void Start()
        {
            _ecsInfoMB = gameObject.GetComponent<EcsInfoMB>();
            _animator = gameObject.GetComponent<Animator>();
        }

        public void Init(EcsWorldInject world)
        {
            _world = world;
            _damagingEventPool = world.Value.GetPool<DamagingEvent>();
            _damagePool = world.Value.GetPool<DamageComponent>();
            _targetablePool = world.Value.GetPool<Targetable>();
        }

        public void DealDamagingEvent()
        {
            ref var damagingEventComponent = ref _damagingEventPool.Add(_world.Value.NewEntity());
            ref var damageComponent = ref _damagePool.Get(_ecsInfoMB.GetEntity());
            ref var targetableComponent = ref _targetablePool.Get(_ecsInfoMB.GetEntity());
            damagingEventComponent.TargetEntity = targetableComponent.TargetEntity;
            damagingEventComponent.DamageValue = damageComponent.Value;
            damagingEventComponent.DamagingEntity = _ecsInfoMB.GetEntity();
        }
    }
}
