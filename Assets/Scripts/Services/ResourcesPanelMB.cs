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
        [SerializeField] private Text _stonePanel;
        [SerializeField] private Text _stoneAmount;
        [SerializeField] private Text _goldPanel;
        [SerializeField] private Text _goldAmount;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
        }
        public void UpdateStone()
        {
            _stoneAmount.text = ($"{_state.RockCount}");
        }
        public void UpdateGold()
        {
            _goldAmount.text = ($"{_state.CoinCount}");
        }
    }
}
