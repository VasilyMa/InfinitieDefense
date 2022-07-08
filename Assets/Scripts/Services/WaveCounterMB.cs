using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    public class WaveCounterMB : MonoBehaviour
    {
        private EcsWorld _world;
        private GameState _state;

        [SerializeField] private Slider _slider;
        [SerializeField] private float _currentAmount;
        [SerializeField] private float _maxAmount;

        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
        }
        public void SetMaxAmount(float value)
        {
            _maxAmount = value;
            _currentAmount = value;
            _slider.maxValue = _maxAmount;
            _slider.value = _currentAmount;

        }
        public void UpdateWave()
        {
            _currentAmount = _maxAmount - _state.GetCurrentWave();
            _slider.value = _currentAmount;
        }
    }
}