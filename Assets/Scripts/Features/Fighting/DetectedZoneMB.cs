using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DetectedZoneMB : MonoBehaviour
    {
        [SerializeField] private EcsInfoMB _ecsInfoMB;
        [SerializeField] private List<GameObject> _detectedEnemyObjects;

        private EcsWorldInject _world;

        private EcsPool<TargetingEvent> _targetingEventPool;

        private string Enemy;

        public void Start()
        {
            _ecsInfoMB = GetComponentInParent<EcsInfoMB>();
            _detectedEnemyObjects = new List<GameObject>();
            Debug.Log("�� �������� ��������������������");
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("���-�� ����� � ��� ���������");
            if (other.isTrigger)
            {
                Debug.Log("��� �����-���� ���-�� �������");
                return;
            }
            if (!other.gameObject.CompareTag(nameof(Enemy)))
            {
                Debug.Log("��� ��� �� ���������");
                return;
            }
            Debug.Log("��� ��� ���������");
            _detectedEnemyObjects.Add(other.gameObject);

            if (_ecsInfoMB.GetTargetObject() == null)
            {
                _ecsInfoMB.SetTargetObject(_detectedEnemyObjects[0]);
            }

            _targetingEventPool = _ecsInfoMB.GetWorld().Value.GetPool<TargetingEvent>();
            ref var targetingEvent = ref _targetingEventPool.Add(_ecsInfoMB.GetWorld().Value.NewEntity());
            targetingEvent.TargetingEntity = _ecsInfoMB.GetEntity();
            targetingEvent.TargetEntity = other.gameObject.GetComponent<EcsInfoMB>().GetEntity();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag(nameof(Enemy)))
            {
                return;
            }

            if (!_ecsInfoMB.GetTargetObject())
            {
                return;
            }

            if (_ecsInfoMB.GetTargetObject().layer == LayerMask.NameToLayer("Dead"))
            {
                if (_ecsInfoMB.GetTargetObject() == _detectedEnemyObjects[0])
                {
                    _detectedEnemyObjects.Remove(_ecsInfoMB.GetTargetObject());
                }

                if (_detectedEnemyObjects.Count > 0)
                {
                    _ecsInfoMB.SetTargetObject(_detectedEnemyObjects[0]);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag(nameof(Enemy)))
            {
                return;
            }

            _detectedEnemyObjects.Remove(other.gameObject);

            if (_detectedEnemyObjects.Count == 0)
            {
                _ecsInfoMB.SetTargetObject(null);
            }
            else
            {
                _ecsInfoMB.SetTargetObject(_detectedEnemyObjects[0]);
            }
        }
    }
}
