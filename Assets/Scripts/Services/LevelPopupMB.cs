using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    public class LevelPopupMB : MonoBehaviour
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
        public void UpdateLevel(int currentLevel)
        {
            if (currentLevel <= 0)
                _levelObject.SetActive(false);
            else
                _levelObject.SetActive(true);
            level = $"New Level {currentLevel}";
            _textAmount.text = level;
        }
        public void UpdateInfoDamageHealth(int damage, int health)
        {
            string info = $"Damage up to {damage} Health up to {health}";
            _textAmount.text = info;
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
        public Text GetText()
        {
            return _textAmount;
        }
    }
}
