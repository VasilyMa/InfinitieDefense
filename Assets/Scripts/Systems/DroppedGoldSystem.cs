using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DroppedGoldSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<DroppedGoldEvent>> _filter = default;
        readonly EcsSharedInject<GameState> _state = default;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);

                GameObject.Instantiate(_state.Value.InterfaceStorage.GoldPrefab, filterComp.Position, Quaternion.identity);

                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}