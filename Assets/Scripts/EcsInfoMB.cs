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

        private EcsPool<TargetingEvent> _targetingEventPool;

        [SerializeField] private int _gameObjectEntity;

        [SerializeField] private int _targetEntity;
        [SerializeField] private GameObject _targetObject;


        public void Init(EcsWorldInject world)
        {
            _world = world;
            _targetingEventPool = world.Value.GetPool<TargetingEvent>();
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

        public void SetTargetObject(GameObject gameObject)
        {
            _targetObject = gameObject;
        }
    }
}
