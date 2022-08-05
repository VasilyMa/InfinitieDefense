using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Client {
    sealed class KillsCounterSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<KillsCountComponent>> _killsFilter = default;
        readonly EcsPoolInject<InterfaceComponent> _interfaceComponent = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _killsFilter.Value)
            {
                ref var interfaceComponent = ref _interfaceComponent.Value.Get(_state.Value.EntityInterface);
                interfaceComponent.killsCounter.GetComponent<KillCounterMB>().UpdateKills();
            }
        }
    }
}