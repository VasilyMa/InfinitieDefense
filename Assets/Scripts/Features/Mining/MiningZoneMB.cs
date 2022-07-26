using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class MiningZoneMB : MonoBehaviour
    {
        [SerializeField] private GameObject _mainGameObject;
        [SerializeField] private EcsInfoMB _ecsInfoMB;
        [SerializeField] private Animator _animator;

        private EcsWorldInject _world;

        private EcsPool<OreComponent> _orePool;

        private string Ore;

        private int _oreInZone;

        private ContextToolComponent.Tool _thisTool = ContextToolComponent.Tool.pickaxe;

        void Start()
        {
            if (_mainGameObject == null) _mainGameObject = transform.parent.gameObject;
            if (_ecsInfoMB == null) _ecsInfoMB = _mainGameObject.GetComponent<EcsInfoMB>();
            if (_animator == null) _animator = _mainGameObject.GetComponent<Animator>();
            _world = _ecsInfoMB.GetWorld();
        }

        private void Update()
        {
            if (_ecsInfoMB.GetCurrentMiningOre() == -1)
            {
                return;
            }

            _orePool = _world.Value.GetPool<OreComponent>();
            ref var oreComponent = ref _orePool.Get(_ecsInfoMB.GetCurrentMiningOre());

            if (oreComponent.prefab.gameObject.activeSelf)
            {
                return;
            }

            _animator.SetBool("isMining", false);

            _world = _ecsInfoMB.GetWorld();

            _ecsInfoMB.SetCurrentMiningOre(-1);

            if (!_animator.GetBool("Attack"))
            {
                _ecsInfoMB.DeactivateContextTool(_thisTool);
            }
        }

        // to do ay realize method ActivateContextToolPool() in EcsInfo

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
            {
                return;
            }

            if (!other.gameObject.CompareTag(nameof(Ore)))
            {
                return;
            }

            _animator.SetBool("isMining", true);

            _world = _ecsInfoMB.GetWorld();

            var oreEcsInfo = other.gameObject.GetComponent<EcsInfoMB>();

            _ecsInfoMB.SetCurrentMiningOre(oreEcsInfo.GetEntity());
            _oreInZone++;

            if (!_animator.GetBool("Attack"))
            {
                _ecsInfoMB.ActivateContextTool(_thisTool);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger)
            {
                return;
            }

            if (!other.gameObject.CompareTag(nameof(Ore)))
            {
                return;
            }

            _animator.SetBool("isMining", false);

            _world = _ecsInfoMB.GetWorld();

            _ecsInfoMB.SetCurrentMiningOre(-1);
            _oreInZone--;

            if (!_animator.GetBool("Attack"))
            {
                _ecsInfoMB.DeactivateContextTool(_thisTool);
            }
        }
    }
}
