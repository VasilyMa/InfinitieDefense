using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
namespace Client 
{
    public class UpgradeCanvasMB : MonoBehaviour
    {
        private EcsWorld _world;
        private GameState _state;
        [SerializeField] private Text _amount;
        [SerializeField] private Image _sprite;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
        }
        public void UpdateUpgradePoint(int currentAmount, int maxAmount, Sprite image)
        {
            _amount.text = ($"{maxAmount - currentAmount}");
            _sprite.sprite = image;
            //Debug.Log(_amount.text);
        }
    }
}

