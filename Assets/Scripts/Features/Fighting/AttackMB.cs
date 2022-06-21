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
        private EcsPool<InFightTag> _inFightPool;

        void Start()
        {
            _ecsInfoMB = gameObject.GetComponent<EcsInfoMB>();
            _animator = gameObject.GetComponent<Animator>();
        }

        private void Update()
        {
            if (_ecsInfoMB.GetTargetObject() == null)
            {
                _animator.SetBool("Attack", false);
            }
        }

        public void Init(EcsWorldInject world)
        {
            _world = world;
            _damagingEventPool = world.Value.GetPool<DamagingEvent>();
            _damagePool = world.Value.GetPool<DamageComponent>();
            _targetablePool = world.Value.GetPool<Targetable>();
            _inFightPool = world.Value.GetPool<InFightTag>();
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _ecsInfoMB.GetTargetObject())
            {
                _animator.SetBool("Attack", true);
                _animator.SetBool("Run", false);

                if (!_inFightPool.Has(_ecsInfoMB.GetEntity()))
                {
                    _inFightPool.Add(_ecsInfoMB.GetEntity());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _ecsInfoMB.GetTargetObject())
            {
                _animator.SetBool("Attack", false);

                if (_inFightPool.Has(_ecsInfoMB.GetEntity()))
                {
                    _inFightPool.Del(_ecsInfoMB.GetEntity());
                }
            }
        }
    }
}
