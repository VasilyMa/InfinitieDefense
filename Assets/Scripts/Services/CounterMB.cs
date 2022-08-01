using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client
{
    public class CounterMB : MonoBehaviour
    {
        public List<Image> points;
        private EcsWorld _world;
        private GameState _state;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
        }
        public void ChangeCount(int index)
        {
            points[index].GetComponent<Image>().color = Color.red;
        }
    }
}