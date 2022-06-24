using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class TowerAttackMB : MonoBehaviour
    {
        [SerializeField] private GameObject _mainGameObject;
        [SerializeField] private EcsInfoMB _ecsInfoMB;

        private EcsWorldInject _world;

        private EcsPool<DamagingEvent> _damagingEventPool;
        private EcsPool<DamageComponent> _damagePool;
        private EcsPool<Targetable> _targetablePool;

        private string _enemyTag = "Enemy";
        private string _friendlyTag = "Friendly";
        private string _targetTag;

        private float MaxCoolDown = 2f;
        private float CurrentCoolDown = 0f;

        private string Enemy;

        void Start()
        {
            if (_mainGameObject == null) _mainGameObject = transform.parent.gameObject;
            if (_ecsInfoMB == null) _ecsInfoMB = _mainGameObject.GetComponent<EcsInfoMB>();

            if (_mainGameObject.CompareTag(_enemyTag))
            {
                _targetTag = _friendlyTag;
            }
            else
            {
                _targetTag = _enemyTag;
            }
        }
        void Update()
        {
            if (CurrentCoolDown > 0)
            {
                CurrentCoolDown -= Time.deltaTime;
            }
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
            if (CurrentCoolDown > 0) return;

            ref var damagingEventComponent = ref _damagingEventPool.Add(_world.Value.NewEntity());
            ref var damageComponent = ref _damagePool.Get(_ecsInfoMB.GetEntity());

            Debug.Log("Членистоногая сущность - "+ _ecsInfoMB.GetEntity()+ " \nСовершает непотребство над - "+ _ecsInfoMB.GetTargetObject().GetComponent<EcsInfoMB>().GetEntity() + " \nС уроном в "+ damageComponent.Value);

            damagingEventComponent.TargetEntity = _ecsInfoMB.GetTargetObject().GetComponent<EcsInfoMB>().GetEntity();
            damagingEventComponent.DamageValue = damageComponent.Value;
            damagingEventComponent.DamagingEntity = _ecsInfoMB.GetEntity();
            CurrentCoolDown = MaxCoolDown;
        }
    }
}
