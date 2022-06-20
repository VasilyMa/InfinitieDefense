using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class TowerAttackMB : MonoBehaviour
    {
        [SerializeField] private EcsInfoMB _ecsInfoMB;
        [SerializeField] private List<GameObject> _detectedEnemyObjects;
        [SerializeField] private GameObject _firstDetectedEnemyObject;

        //private Animator _animator;

        private EcsWorldInject _world;

        private EcsPool<DamagingEvent> _damagingEventPool;
        private EcsPool<DamageComponent> _damagePool;
        private EcsPool<Targetable> _targetablePool;

        private float MaxCoolDown = 2f;
        private float CurrentCoolDown = 0f;

        private string Enemy;

        void Start()
        {
            _ecsInfoMB = gameObject.GetComponent<EcsInfoMB>();
            //_animator = gameObject.GetComponent<Animator>();
            _detectedEnemyObjects = new List<GameObject>();
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

            Debug.Log("Членистоногая сущность - "+ _ecsInfoMB.GetEntity()+ " \nСовершает непотребство над - "+ _firstDetectedEnemyObject.GetComponent<EcsInfoMB>().GetEntity() + " \nС уроном в "+ damageComponent.Value);

            damagingEventComponent.TargetEntity = _firstDetectedEnemyObject.GetComponent<EcsInfoMB>().GetEntity();
            damagingEventComponent.DamageValue = damageComponent.Value;
            damagingEventComponent.DamagingEntity = _ecsInfoMB.GetEntity();
            CurrentCoolDown = MaxCoolDown;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(nameof(Enemy)))
            {
                return;
            }

            if (!_firstDetectedEnemyObject)
            {
                _firstDetectedEnemyObject = other.gameObject;
            }

            _detectedEnemyObjects.Add(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag(nameof(Enemy)))
            {
                return;
            }

            if (!_firstDetectedEnemyObject)
            {
                _firstDetectedEnemyObject = _detectedEnemyObjects[0];
            }

            DealDamagingEvent();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag(nameof(Enemy)))
            {
                return;
            }

            if (other.gameObject == _firstDetectedEnemyObject)
            {
                _firstDetectedEnemyObject = null;
            }

            _detectedEnemyObjects.Remove(other.gameObject);
        }
    }
}
