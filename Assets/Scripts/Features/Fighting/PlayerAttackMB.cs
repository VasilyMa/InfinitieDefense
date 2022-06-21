using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class PlayerAttackMB : MonoBehaviour
    {
        [SerializeField] private EcsInfoMB _ecsInfoMB;
        [SerializeField] private List<GameObject> _detectedEnemyObjects;

        private Animator _animator;

        private EcsWorldInject _world;

        private EcsPool<DamagingEvent> _damagingEventPool;

        private string Enemy;

        public void Start()
        {
            _ecsInfoMB = gameObject.GetComponent<EcsInfoMB>();
            _animator = gameObject.GetComponent<Animator>();
            _detectedEnemyObjects = new List<GameObject>();
        }

        public void Init(EcsWorldInject world)
        {
            _world = world;
            _damagingEventPool = world.Value.GetPool<DamagingEvent>();
        }

        public void DealDamagingEvent()
        {
            if (!_detectedEnemyObjects[0])
            {
                return;
            }

            ref var damagingEventComponent = ref _damagingEventPool.Add(_world.Value.NewEntity());
            damagingEventComponent.TargetEntity = _detectedEnemyObjects[0].GetComponent<EcsInfoMB>().GetEntity();
            damagingEventComponent.DamageValue = 50;
            damagingEventComponent.DamagingEntity = _ecsInfoMB.GetEntity();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(nameof(Enemy)))
            {
                if (_detectedEnemyObjects.Count > 0)
                {
                    _detectedEnemyObjects.Clear();
                }
                return;
            }

            _detectedEnemyObjects.Add(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag(nameof(Enemy)))
            {
                return;
            }

            if (_detectedEnemyObjects.Count == 0)
            {
                _detectedEnemyObjects.Add(other.gameObject);
            }

            _animator.SetBool("Attack", true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag(nameof(Enemy)))
            {
                return;
            }

            if (other.gameObject == _detectedEnemyObjects[0])
            {
                _animator.SetBool("Attack", false);
            }

            _detectedEnemyObjects.Remove(other.gameObject);
        }
    }
}
