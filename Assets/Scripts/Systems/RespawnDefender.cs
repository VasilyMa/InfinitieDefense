using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class RespawnDefender : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        private float _timer = 0;
        public void Run (EcsSystems systems) {
            _timer += Time.deltaTime;
            if(_timer >= 4f)
            {
                for (int i = 0; i < _state.Value.DefendersGOs.Length;i++)
                {
                    if(_state.Value.DefendersGOs[i] != null && !_state.Value.DefendersGOs[i].activeSelf)
                    {
                        _state.Value.DefendersGOs[i].SetActive(true);
                        _timer = 0;
                        return;
                    }
                }
            }
        }
    }
}