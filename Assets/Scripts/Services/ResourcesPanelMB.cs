using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
namespace Client
{
    public class ResourcesPanelMB : MonoBehaviour
    {
        private EcsWorld _world;
        private GameState _state;
        private Animator _stoneAnimator;
        private Animator _goldAnimator;
        [SerializeField] private GameObject _stonePanel;
        [SerializeField] private Text _stoneAmount;
        [SerializeField] private GameObject _goldPanel;
        [SerializeField] private Text _goldAmount;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
        }

        private void Start()
        {
            _stoneAnimator = _stonePanel.GetComponent<Animator>();
            _goldAnimator = _goldPanel.GetComponent<Animator>();
        }

        public void UpdateStone()
        {
            _stoneAmount.text = ($"{_state.RockCount}");
        }
        public void UpdateGold()
        {
            _goldAmount.text = ($"{_state.CoinCount}");
        }

        public void EnableStonePanel()
        {
            if (!_stonePanel.activeSelf)
            {
                _stonePanel.SetActive(true);
                Debug.Log("Test");
                _stoneAnimator.SetTrigger("Enable");
            }
        }

        public void DisableStonePanel()
        {
            if (_stonePanel.activeSelf) _stonePanel.SetActive(false);
        }

        public void EnableGoldPanel()
        {
            if (!_goldPanel.activeSelf)
            {
                _goldPanel.SetActive(true);
                _goldAnimator.SetTrigger("Enable");
            }
        }

        public void DisableGoldPanel()
        {
            if (_goldPanel.activeSelf) _goldPanel.SetActive(false);
        }
    }
}
