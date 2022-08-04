using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
namespace Client
{
    public class HealthbarMB : MonoBehaviour
    {
        private EcsWorld _world;
        private GameState _state;
        [SerializeField] private Slider _slider;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Image _image;
        [SerializeField] private float _curHp;
        [SerializeField] private float _maxHP;
        [SerializeField] private GameObject _healthBar;

        private EcsPool<CameraComponent> _cameraPool = null;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
            _cameraPool = _world.GetPool<CameraComponent>();
        }
        public void SetMaxHealth(float health)
        {
            _slider.maxValue = health;
            _slider.value = health;
            _maxHP = health;
            _image.color = _gradient.Evaluate(1f);
        }
        public void SetHealth(float health)
        {
            _slider.value = health;
            _curHp = health;
        }
        public void UpdateHealth(float currentHP)
        {
            _curHp = currentHP;
            _slider.value = _curHp;
            _image.color = _gradient.Evaluate(_slider.normalizedValue);
            if (_slider.value <= 0)
                _healthBar.SetActive(false);
        }
        private void CameraFollow()
        {
            ref var cameraComp = ref _cameraPool.Get(_state.EntityCamera);
            _healthBar.transform.LookAt(_healthBar.transform.position + cameraComp.CameraTransform.forward);
        }
        public void ToggleSwitcher()
        {
            _healthBar.SetActive(!_healthBar.activeSelf);
        }
        public void Disable()
        {
            if (_healthBar.activeSelf) _healthBar.SetActive(false);
        }
        public void Enableble()
        {
            if (!_healthBar.activeSelf) _healthBar.SetActive(true);
        }
        private void Update()
        {
            CameraFollow();
            if (_curHp == _maxHP || _curHp <= 0)
                _healthBar.SetActive(false);
            else
                _healthBar.SetActive(true);
        }
    }
}
