using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using UnityEngine.UI;

namespace Client 
{
    public class PlayerUpgradePointMB : MonoBehaviour
    {
        private EcsWorld _world;
        private GameState _state;
        [SerializeField] private Text _textAmount;
        [SerializeField] private GameObject _levelObject;
        private string level;
        private EcsPool<CameraComponent> _cameraPool = null;

        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
            _cameraPool = _world.GetPool<CameraComponent>();
        }
        private void CameraFollow()
        {
            ref var cameraComp = ref _cameraPool.Get(_state.EntityCamera);
            _levelObject.transform.LookAt(_levelObject.transform.position + cameraComp.CameraTransform.forward);
        }
        public void UpdateLevelInfo(float maxAmount, float currentAmount)
        {
            _textAmount.text = ($"{maxAmount - currentAmount}");
        }
        private void Update()
        {
            CameraFollow();
        }
    }
}

