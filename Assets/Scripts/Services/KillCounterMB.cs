using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client
{
    public class KillCounterMB : MonoBehaviour
    {
        [SerializeField] private Text _killsCount;
        private EcsPool<KillsCountComponent> _killsPool;
        private EcsWorld _world;
        private GameState _state;
        private float curentKills;
        private float _time;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
            _killsPool = _world.GetPool<KillsCountComponent>();
        }

        public void UpdateKills()
        {
            Kills();
        }
        void Kills()
        {
            curentKills += Time.deltaTime * 3;
            if (curentKills > _state.KillsCount + 1)
            {
                var filter = _world.Filter<KillsCountComponent>();
                foreach (int entity in filter.End())
                {
                    if (_killsPool.Has(entity))
                    {
                        _world.DelEntity(entity);
                    }
                }
            }
            else
            {
                float kills = Mathf.FloorToInt(curentKills);
                _killsCount.text = string.Format("{00}", kills);
            }
        }
    }
}
