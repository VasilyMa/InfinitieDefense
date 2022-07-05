using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class LoseSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<LoseEvent>> _loseFilter = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        readonly EcsSharedInject<GameState> _state;
        public void Run (EcsSystems systems) {
            foreach (var entity in _loseFilter.Value)
            {
                ref var interfaceComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                Time.timeScale = 0;
                interfaceComp.losePanel.SetActive(true);
            }
        }
    }
}