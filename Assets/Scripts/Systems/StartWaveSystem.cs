using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class StartWaveSystem : IEcsInitSystem {
        readonly EcsPoolInject<StartWaveEvent> _pool = default;
        readonly EcsWorldInject _world = default;
        public void Init (EcsSystems systems) {
            _pool.Value.Add(_world.Value.NewEntity());
        }
    }
}