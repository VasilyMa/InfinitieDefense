using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class EcsInfoMB : MonoBehaviour
    {
        private EcsWorldInject _world;

        private EcsPool<Targetable> _targetablePool;
        private EcsPool<DamagingEvent> _damagingEventPool;
        private EcsPool<DamageComponent> _damagePool;

        [SerializeField] private int _gameObjectEntity;

        [SerializeField] private int _targetEntity;
        [SerializeField] private GameObject _targetObject;

        public void Init(EcsWorldInject world)
        {
            _world = world;
            _targetablePool = world.Value.GetPool<Targetable>();
            _damagingEventPool = world.Value.GetPool<DamagingEvent>();
            _damagePool = world.Value.GetPool<DamageComponent>();
        }

        public void SetEntity(int entity)
        {
            _gameObjectEntity = entity;
        }

        public int GetEntity()
        {
            return _gameObjectEntity;
        }

        public EcsWorldInject GetWorld()
        {
            return _world;
        }

        public GameObject GetTargetObject()
        {
            return _targetObject;
        }

        public void SetTarget(int entity, GameObject gameObject)
        {
            if (gameObject == null) Debug.Log("Мы очищаем инфо о gameObgect нашей цели.");
            if (entity == -1) Debug.Log("Мы очищаем инфо о entity нашей цели.");
            _targetEntity = entity;
            _targetObject = gameObject;
        }
        public void SetTarget(GameObject gameObject)
        {
            if (gameObject == null)
            {
                Debug.Log("Мы не можем очистить gameObject в этом методе");
                return;
            }
            _targetObject = gameObject;
        }
        public void SetTarget(int entity)
        {
            if (entity == -1)
            {
                Debug.Log("Мы не можем очистить entity в этом методе");
                return;
            }
            _targetEntity = entity;
        }
        public void ResetTarget()
        {
            _targetEntity = -1;
            _targetObject = null;
        }

        public void DealDamagingEvent()
        {
            _world = GetWorld();
            _damagingEventPool = _world.Value.GetPool<DamagingEvent>();

            ref var damagingEventComponent = ref _damagingEventPool.Add(_world.Value.NewEntity());
            ref var damageComponent = ref _damagePool.Get(_gameObjectEntity);
            ref var targetableComponent = ref _targetablePool.Get(_gameObjectEntity);
            damagingEventComponent.TargetEntity = targetableComponent.TargetEntity;
            damagingEventComponent.DamageValue = damageComponent.Value;
            damagingEventComponent.DamagingEntity = _gameObjectEntity;
        }
    }
}
