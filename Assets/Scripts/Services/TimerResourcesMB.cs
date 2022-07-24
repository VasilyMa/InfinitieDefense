using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
namespace Client {
    public class TimerResourcesMB : MonoBehaviour
    {
        private EcsWorld _world;
        private GameState _state;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _levelObject;
        private string level;
        private EcsPool<CameraComponent> _cameraPool = null;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
            _cameraPool = _world.GetPool<CameraComponent>();
        }
        public void ResourcesDrop(float resourcesTimer)
        {
            _image.fillAmount = resourcesTimer / 2;
            //_image.fillAmount = Mathf.Lerp(_image.fillAmount, resourcesTimer, Time.deltaTime * 5);
        }
        private void CameraFollow()
        {
            ref var cameraComp = ref _cameraPool.Get(_state.EntityCamera);
            _levelObject.transform.LookAt(_levelObject.transform.position + cameraComp.CameraTransform.forward);
        }

        private void Update()
        {
            CameraFollow();
        }
    }
}

