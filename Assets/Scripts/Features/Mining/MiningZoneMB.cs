using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;

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
        private List<int> _oresEntityInZone;

        private ContextToolComponent.Tool _thisTool = ContextToolComponent.Tool.pickaxe;

        void Start()
        {
            if (_mainGameObject == null) _mainGameObject = transform.parent.gameObject;
            if (_ecsInfoMB == null) _ecsInfoMB = _mainGameObject.GetComponent<EcsInfoMB>();
            if (_animator == null) _animator = _mainGameObject.GetComponent<Animator>();
            _world = _ecsInfoMB.GetWorld();
            _oresEntityInZone = new List<int>(); 
        }

        private void Update()
        {
            if (_ecsInfoMB.GetCurrentMiningOre() == -1)
            {
                return;
            }

            _orePool = _world.Value?.GetPool<OreComponent>();
            ref var oreComponent = ref _orePool.Get(_ecsInfoMB.GetCurrentMiningOre());

            if (oreComponent.prefab.gameObject.activeSelf)
            {
                return;
            }

            _oresEntityInZone.Remove(_ecsInfoMB.GetCurrentMiningOre());

            RetargetOre();
            SwitchMiningTool();
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

            var oreEcsInfo = other.gameObject.GetComponent<EcsInfoMB>();

            _oresEntityInZone.Add(oreEcsInfo.GetEntity());

            RetargetOre();
            SwitchMiningTool();
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

            _oresEntityInZone.Remove(_ecsInfoMB.GetCurrentMiningOre());

            RetargetOre();
            SwitchMiningTool();
        }

        private void SwitchMiningTool()
        {
            if (_oresEntityInZone.Count > 0)
            {
                _animator.SetBool("isMining", true);
                _ecsInfoMB.ActivateContextTool(_thisTool);
            }
            else
            {
                _animator.SetBool("isMining", false);
                _ecsInfoMB.DeactivateContextTool(_thisTool);
            }
        }

        private void RetargetOre()
        {
            if (_oresEntityInZone.Count > 0)
            {
                _ecsInfoMB.SetCurrentMiningOre(_oresEntityInZone[0]);
            }
            else
            {
                _ecsInfoMB.SetCurrentMiningOre(-1);
            }
        }
    }
}
