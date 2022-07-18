using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class RangeAttackMB : MonoBehaviour
    {
        [SerializeField] private GameObject _mainGameObject;
        [SerializeField] private EcsInfoMB _ecsInfoMB;
        [SerializeField] private Animator _animator;

        private EcsWorldInject _world;

        private EcsPool<Targetable> _targetablePool;
        private EcsPool<ActivateContextToolEvent> _activateContextToolPool;
        private EcsPool<ContextToolComponent> _contextToolPool;

        private string _enemyTag = "Enemy";
        private string _friendlyTag = "Friendly";
        private string _targetTag;

        private ContextToolComponent.Tool _thisTool = ContextToolComponent.Tool.bow;

        void Start()
        {
            if (_mainGameObject == null) _mainGameObject = transform.parent.gameObject;
            if (_ecsInfoMB == null) _ecsInfoMB = _mainGameObject.GetComponent<EcsInfoMB>();
            if (_animator == null) _animator = _mainGameObject.GetComponent<Animator>();

            _animator.SetBool("Range", true);

            if (_mainGameObject.CompareTag(_enemyTag))
            {
                _targetTag = _friendlyTag;
            }
            else
            {
                _targetTag = _enemyTag;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
            {
                return;
            }

            if (!other.gameObject.CompareTag(_targetTag))
            {
                return;
            }

            _world = _ecsInfoMB.GetWorld();
            _targetablePool = _world.Value.GetPool<Targetable>();
            _activateContextToolPool = _world.Value.GetPool<ActivateContextToolEvent>();
            _contextToolPool = _world.Value.GetPool<ContextToolComponent>();

            ref var targetableComponent = ref _targetablePool.Get(_ecsInfoMB.GetEntity());
            targetableComponent.AllEntityInDamageZone.Add(other.GetComponent<EcsInfoMB>().GetEntity());

            if (!_contextToolPool.Has(_ecsInfoMB.GetEntity()))
            {
                return;
            }

            ref var contextToolComponent = ref _contextToolPool.Get(_ecsInfoMB.GetEntity());

            if (contextToolComponent.CurrentActiveTool == _thisTool)
            {
                return;
            }

            ref var activateContextToolEvent = ref _activateContextToolPool.Add(_ecsInfoMB.GetEntity());
            activateContextToolEvent.ActiveTool = _thisTool;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger)
            {
                return;
            }

            if (!other.gameObject.CompareTag(_targetTag))
            {
                return;
            }

            _world = _ecsInfoMB.GetWorld();
            _targetablePool = _world.Value.GetPool<Targetable>();
            ref var targetableComponent = ref _targetablePool.Get(_ecsInfoMB.GetEntity());
            targetableComponent.AllEntityInDamageZone.Remove(other.GetComponent<EcsInfoMB>().GetEntity());
        }
    }
}
