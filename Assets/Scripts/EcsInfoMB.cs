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

        [SerializeField] private int _gameObjectEntity;

        public void Init(EcsWorldInject world)
        {
            _world = world;
        }

        public void SetEntity(int entity)
        {
            _gameObjectEntity = entity;
        }

        public int GetUnitEntity()
        {
            return _gameObjectEntity;
        }
    }
}
