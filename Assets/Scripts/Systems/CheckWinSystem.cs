using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class CheckWinSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<EnemyTag, UnitTag, DeadTag>> _filterEnemies = default;
        readonly EcsFilterInject<Inc<CountdownWaveComponent>> _countdown = default;
        readonly EcsWorldInject _world;
        readonly EcsSharedInject<GameState> _state;
        readonly EcsPoolInject<CountdownWaveComponent> _countdownPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        public void Run (EcsSystems systems) {
            foreach (var item in _filterEnemies.Value)
            {
                ref var interfaceComponent = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                if (_filterEnemies.Value.GetEntitiesCount() == _state.Value.WaveStorage.GetAllEnemies() && _state.Value.isWave)
                {
                    foreach (var countdown in _countdown.Value)
                    {
                        _countdown.Pools.Inc1.Del(countdown);
                    }
                    _state.Value.isWave = false;
                    _countdownPool.Value.Add(_world.Value.NewEntity());
                    interfaceComponent.countdownWave.GetComponent<CountdownWaveMB>().SetTimer(30);
                    interfaceComponent.countdownWave.GetComponent<CountdownWaveMB>().SetText("Next lvl!");
                }
            }
        }
    }
}