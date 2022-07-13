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
        [SerializeField] private GameObject _container;
        [SerializeField] private List<GameObject> counts = new List<GameObject>();
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
            for (int i = 0; i < _container.transform.childCount; i++)
            {
                counts.Add(_container.transform.GetChild(i).gameObject);
                counts[i].SetActive(false);
            }
            for (int i = 0; i < value - 1; i++)
            {
                counts[i].SetActive(true);
            }
            for (int i = 0; i < counts.Count; i++)
            {
                if(!counts[i].activeSelf)
                    _container.GetComponent<GridLayoutGroup>().spacing = new Vector2(_container.GetComponent<GridLayoutGroup>().spacing.x + 7.5f, 0);
            }
            
        }
        public void UpdateWave()
        {
            if (_currentAmount>=0)
                _currentAmount--;
            _slider.value = _currentAmount;
            for (int i = 0; i < counts.Count; i++)
            {
                if (counts[i].activeSelf)
                {
                    counts[i].SetActive(false);
                    if (!counts[i].activeSelf)
                        _container.GetComponent<GridLayoutGroup>().spacing = new Vector2(_container.GetComponent<GridLayoutGroup>().spacing.x * 1.5f, 0);
                    break;
                }
            }
        }
    }
}