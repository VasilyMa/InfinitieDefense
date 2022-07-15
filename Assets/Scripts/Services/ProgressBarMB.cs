using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    public class ProgressBarMB : MonoBehaviour
    {
        private EcsWorld _world;
        private GameState _state;
        [SerializeField] private Slider _slider;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Image _fill;
        [SerializeField] private float _currentAmount;
        [SerializeField] private float _maxAmount;
        [SerializeField] private GameObject _progress;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
        }
        public void SetMaxAmount(float value)
        {
            _maxAmount = value;
            _currentAmount = value - value;
            _slider.maxValue = _maxAmount;
            _slider.value = _currentAmount;

        }
        public void UpdateProgressBar()
        {
            var filter = _world.Filter<EnemyTag>().Inc<DeadTag>().End();
            _currentAmount++;
            //_currentAmount = _state.WaveStorage.GetAllEnemies() - filter.GetEntitiesCount();
            _slider.value = _currentAmount;
        }
    }
}
