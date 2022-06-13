using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class StoneMiningSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<StoneMiningEvent>> _filter = default;

        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _state.Value.RockCount++;
                //todo добавить перемещение камня за спину
            }
        }
    }
}