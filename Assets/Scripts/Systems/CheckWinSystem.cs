using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class CheckWinSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<EnemyTag, UnitTag, DeadTag>> _filterEnemies = default;
        readonly EcsWorldInject _world;
        readonly EcsSharedInject<GameState> _state;
        readonly EcsPoolInject<WinEvent> _winPool = default;
        public void Run (EcsSystems systems) {
            foreach (var item in _filterEnemies.Value)
            {
                Debug.Log($"AllEnemies {_state.Value.WaveStorage.GetAllEnemies()}" +
                    $"DeadEnemies { _filterEnemies.Value.GetEntitiesCount() }");
                if (_filterEnemies.Value.GetEntitiesCount() == _state.Value.WaveStorage.GetAllEnemies())
                {
                    Debug.Log("Win!");
                    _winPool.Value.Add(_world.Value.NewEntity());
                }
            }
        }
    }
}