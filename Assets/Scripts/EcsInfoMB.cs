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

        private EcsPool<ViewComponent> _viewPool;
        private EcsPool<Projectile> _projectilePool;

        [SerializeField] private int _gameObjectEntity;

        [SerializeField] private int _targetEntity;
        [SerializeField] private GameObject _targetObject;
        [SerializeField] private GameObject _arrowFirePoint;

        public void Init(EcsWorldInject world)
        {
            _world = world;
            _targetablePool = world.Value.GetPool<Targetable>();
            _damagingEventPool = world.Value.GetPool<DamagingEvent>();
            _damagePool = world.Value.GetPool<DamageComponent>();
            _viewPool = world.Value.GetPool<ViewComponent>();
            _projectilePool = world.Value.GetPool<Projectile>();
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

        public void ArrowShooting()
        {
            _world = GetWorld();
            int arrowEntity = _world.Value.NewEntity();

            ref var targetableComponent = ref _targetablePool.Get(GetEntity());
            ref var damageComponent = ref _damagePool.Get(GetEntity());

            ref var arrowViewComponent = ref _viewPool.Add(arrowEntity);
            ref var projectileComponent = ref _projectilePool.Add(arrowEntity);
            ref var arrowDamageComponent = ref _damagePool.Add(arrowEntity);

            arrowViewComponent.GameObject = GameObject.Instantiate(Resources.Load<GameObject>("Test"), _arrowFirePoint.transform.position, Quaternion.identity);

            projectileComponent.Speed = 30;
            projectileComponent.SpeedDecreaseFactor = 1.2f;
            projectileComponent.SpeedIncreaseFactor = 0.8f;
            projectileComponent.OwnerEntity = GetEntity();
            projectileComponent.StartPosition = _arrowFirePoint.transform.position;
            projectileComponent.SupportPosition = Vector3.Lerp(gameObject.transform.position, targetableComponent.TargetObject.transform.position, 0.5f) + new Vector3(0, 5, 0);
            projectileComponent.TargetEntity = targetableComponent.TargetEntity;
            projectileComponent.TargetObject = targetableComponent.TargetObject;

            arrowDamageComponent.Value = damageComponent.Value;
        }
    }
}
