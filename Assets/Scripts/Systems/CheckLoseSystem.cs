using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class CheckLoseSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<Player, ViewComponent, HealthComponent>> _filerPlayer = default;
        readonly EcsFilterInject<Inc<MainTowerTag, ViewComponent>> _filterMainTower = default;
        readonly EcsPoolInject<LoseEvent> _losePool = default;
        readonly EcsWorldInject _world;
        public void Run (EcsSystems systems) {
            foreach (var player in _filerPlayer.Value)
            {
                ref var healthComp = ref _filerPlayer.Pools.Inc3.Get(player);
                if (healthComp.CurrentValue <= 0)
                {
                    _losePool.Value.Add(_world.Value.NewEntity());
                    Debug.Log("Потрачено");
                }
            }
            foreach (var mainTower in _filterMainTower.Value)
            {
                ref var healthComp = ref _filerPlayer.Pools.Inc3.Get(mainTower);
                if (healthComp.CurrentValue <= 0)
                {
                    _losePool.Value.Add(_world.Value.NewEntity());
                    Debug.Log("Потрачено");
                }
            }
        }
    }
}